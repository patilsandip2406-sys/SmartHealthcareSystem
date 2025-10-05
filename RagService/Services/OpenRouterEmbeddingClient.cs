using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace RagService.API.Services
{
    public class OpenRouterEmbeddingClient :IEmbeddingClient
    {
        private readonly IHttpClientFactory _httpFactory;
        private readonly string _apiKey;
        private readonly IConfiguration _config;
        private readonly string _model;
        public OpenRouterEmbeddingClient(IHttpClientFactory httpFactory, IConfiguration config)
        {
            _httpFactory = httpFactory;
            _config = config;
            _apiKey = _config["OpenRouter:ApiKey"];
            _model = _config["OpenRouter:Model"] ?? "gpt-4o-mini";
        }

        public async Task<float[]> GetEmbeddingAsync(string Input, CancellationToken ct)
        {
            #region NR COde
            //var apiKey = _config["OpenRouter:ApiKey"];
            //var http = _httpFactory.CreateClient();
            //http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            ////var payload = new { model = "text-embedding-3-large", input=Input};


            //var payload = new
            //{
            //    model = _model, //_config["OpenRouter:Model"] ?? "gpt-4o-mini",
            //    messages = new[] {
            //        new { role = "system", content = "You are a medical assistant summarizing patient records." },
            //        new { role = "user", content = Input }
            //    }
            //};

            //var resp = await http.PostAsync("https://openrouter.ai/api/v1/chat/completions",   /* //  https://openrouter.ai/api/v1/c*/
            //    new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"));
            //resp.EnsureSuccessStatusCode();

            //var json = await resp.Content.ReadAsStringAsync(ct);
            //using var doc = JsonDocument.Parse(json);

            //var arr1 = doc.RootElement.GetProperty("data")[0];
            //var re = arr1.GetProperty("embedding").EnumerateArray();
            //float[] arr = null;

            ////var list = arr.Select(x=>x.GetSingle()).ToArray();
            ////return list;
            //return arr;

            //var hfToken = _config["OpenRouter:hfkey"];
            //string model = _config["OpenRouter:hfModel"];
            //using var http = new HttpClient();
            //http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", hfToken);
            //var payload = new
            //{
            //    inputs = Input
            //};

            //var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            //var response = await http.PostAsync($"https://api-inference.huggingface.co/pipeline/feature-extraction/{model}", content);
            //response.EnsureSuccessStatusCode();

            //var json = await response.Content.ReadAsStringAsync();
            //using var doc = JsonDocument.Parse(json);

            //// The API returns a 2D array [ [0.1, 0.2, ...] ], so we take the first element
            //var arr = doc.RootElement[0].EnumerateArray().Select(x => x.GetSingle()).ToArray();

            //return arr;

            #endregion

            var _jinaApiKey = _config["OpenRouter:jinaApiKey"];
            string _model = "jina-embeddings-v2-base-en";

            var http = _httpFactory.CreateClient();
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jinaApiKey);

            var payload = new
            {
                input = Input,
                model = _model
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var resp = await http.PostAsync("https://api.jina.ai/v1/embeddings", content, ct);
            resp.EnsureSuccessStatusCode();

            var json = await resp.Content.ReadAsStringAsync(ct);
            using var doc = JsonDocument.Parse(json);

            // JSON structure: { "data": [ { "embedding": [0.12, 0.34, ...] } ] }
            var arr = doc.RootElement.GetProperty("data")[0].GetProperty("embedding")
                .EnumerateArray().Select(x => x.GetSingle()).ToArray();

            return arr;


        }

        public async Task<string> GetCompletionAsync(string prompt, CancellationToken ct)
        {
            var apiKey = _config["OpenRouter:ApiKey"];

            var http = _httpFactory.CreateClient();
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var payload = new
            {
                model = _model, //_config["OpenRouter:Model"] ?? "gpt-4o-mini",
                messages = new[] {
                    new { role = "system", content = "You are a medical assistant summarizing patient records." },
                    new { role = "user", content = prompt }
                }
            };

            var resp = await http.PostAsync("https://openrouter.ai/api/v1/chat/completions",
           new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"), ct);

            resp.EnsureSuccessStatusCode();

            var json = await resp.Content.ReadAsStringAsync(ct);
            using var doc = JsonDocument.Parse(json);

            var content = doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
            return content ?? string.Empty;
        }
    }
}
