using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AIService.Services
{
    public class OpenAIService : IAIService
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        public OpenAIService(IHttpClientFactory factory, IConfiguration config)
        {
            _http = factory.CreateClient();
            _config = config;
        }

        public async Task<string> PatientSummary(string patientInfo, CancellationToken ct = default)
        {
            var apiKey = _config["OpenRouter:ApiKey"];
            // 2. Send to OpenRouter API
            var requestBody = new
            {
                model = "gpt-4.1-mini",
                messages = new[]
                {
                new { role = "system", content = "You are a medical assistant summarizing patient records." },
                new { role = "user", content = $"Summarize the condition of: {patientInfo}" }
            }
            };

            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

            var response = await _http.PostAsJsonAsync("https://openrouter.ai/api/v1/chat/completions", requestBody);
            var aiResult = await response.Content.ReadFromJsonAsync<JsonElement>();

            var aiSummary = aiResult.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();

            return aiSummary ?? "No summary available";
        }

        public async Task<string> ChatAsync(string userMessage, CancellationToken ct = default)
        {
            var apiKey = _config["OpenAI:ApiKey"];
            var model = _config["OpenAI:Model"];
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var payload = new
            {
                model,
                messages = new[] { new { role = "user", content = userMessage } },
                max_tokens = 500
            };

            var resp = await _http.PostAsync("https://api.openai.com/v1/chat/completions",
                new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"), ct);

            var json = await resp.Content.ReadAsStringAsync(ct);
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString()
                   ?? "No response";
        }

        public async Task<string> SummarizePatientAsync(int patientId, CancellationToken ct = default)
        {
            return await ChatAsync($"Summarize patient with ID {patientId} from our records", ct);
        }
    }
}
