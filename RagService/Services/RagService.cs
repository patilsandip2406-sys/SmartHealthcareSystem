using RagService.API.Models;
using RagService.Services;

namespace RagService.API.Services
{
    public class RagServices : IRagService
    {
        private readonly IRetrievalService _retrieval;
        private readonly IEmbeddingClient _embClient;
        private readonly IHttpClientFactory _httpFactory;
        private readonly IDocumentStore _store;

        public RagServices(IRetrievalService retrieval, IEmbeddingClient embClient, IHttpClientFactory httpFactory, IDocumentStore store)
        {
            _retrieval = retrieval;
            _embClient = embClient;
            _httpFactory = httpFactory;
            _store = store;
        }

        public async Task<ChatResponse> PatientSummaryAsync(int patientId, CancellationToken ct = default)
        {
            var client = _httpFactory.CreateClient("patient");
            var resp = await client.GetAsync($"/api/patients/{patientId}", ct);

            if (!resp.IsSuccessStatusCode)
            {
                return new ChatResponse($"Failed to get patient summary for ID {patientId}", new List<QueryResponse>());
            }

            var patientJson = await resp.Content.ReadAsStringAsync(ct);

            var query = $"Provide a clinical summary for patient with data: {patientJson}";

            return await QueryAsync(query, k:4, ct:ct);
        }

        

        public async Task<ChatResponse> QueryAsync(string query, int k = 4, CancellationToken ct = default)
        {
            var top = await _retrieval.RetrieveTopKAsync(query, k, ct);
            var context = string.Join("\n---\n", top.Select(x=>x.doc.Content).Take(k));

            var prompt = $@"You are helpful clinical assistant. Use the following context to answer concisely.
            Context : {context}
            Question:{query}
            
            Answer with:
            1) One-line summary
            2) Key findings
            3) Suggested next steps (3 bullets)";

            var answer = await _embClient.GetCompletionAsync(prompt, ct);
            return new ChatResponse(answer, top.Select(x => new QueryResponse(x.doc, x.score)).ToList());
        }

        public async Task<RagResponse> GetPatientNotesAsync(int patientId, string query)
        {
            var notes = (await _store.GetAllNotesAsync(patientId)).Where(x=> x.Embedding != null).ToList();
            var qemb = await _embClient.GetEmbeddingAsync(query, CancellationToken.None);

            float Cosine(float[] a, float[] b)
            {
                if (a.Length != b.Length) throw new ArgumentException("Vectors must be of same length");
                float dot = 0, magA = 0, magB = 0;
                for (int i = 0; i < a.Length; i++)
                {
                    dot += a[i] * b[i];
                    magA += a[i] * a[i];
                    magB += b[i] * b[i];
                }
                return dot / ((float)(Math.Sqrt(magA) * Math.Sqrt(magB)) + 1e-10f);
            }

            var topNotes = notes.Select(n => (n, Cosine(qemb, n.Embedding!)))
                           .OrderByDescending(x => x.Item2)
                           .Take(5)
                           .ToList();

            var context = string.Join("\n---\n", topNotes.Select(x => x.n.Note));
            var prompt = $"You are a helpful assistant. Context:\n{context}\nQuestion: {query}\nAnswer briefly.";
            var answer = await _embClient.GetCompletionAsync(prompt, CancellationToken.None);
            return new RagResponse(answer, topNotes.Select(x => x.n).ToList());
        }
    }
}
