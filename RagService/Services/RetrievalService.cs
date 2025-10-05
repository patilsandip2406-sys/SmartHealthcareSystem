using RagService.API.Models;
using RagService.Services;
using System.Text.Json;

namespace RagService.API.Services
{
    public class RetrievalService : IRetrievalService
    {
        private readonly IEmbeddingClient _emb;
        private readonly IDocumentStore _doc;

        public RetrievalService(IEmbeddingClient embeddingClient, IDocumentStore documentStore)
        {
            _emb = embeddingClient;
            _doc = documentStore;
        }

        public async Task<List<(DocumentDto doc, float score)>> RetrieveTopKAsync(string query, int k = 5, CancellationToken ct = default)
        {
            var qemb = await _emb.GetEmbeddingAsync(query, ct);
            var docs = (await _doc.GetAllDocumentsAsync(ct)).Where(d => d.Embedding != null).ToList();

            var scored = docs.Select(d =>
            {
                var score = CosineSimilarity(qemb, d.Embedding!);
                return (d, score);
            })
            .OrderByDescending(x => x.score)
            .Take(k)
            .ToList();

            return scored;
        }

        private float CosineSimilarity(float[] qemb, float[] floats)
        {
            double dot = 0, na = 0, nb = 0;
            var len = Math.Min(qemb.Length, floats.Length);
            for (int i = 0; i < len; i++)
            {
                dot += qemb[i] * floats[i];
                na += qemb[i] * qemb[i];
                nb += floats[i] * floats[i];
            }

            return (float)(dot / (Math.Sqrt(na) * Math.Sqrt(nb) + 1e-8));
        }
    }
}
