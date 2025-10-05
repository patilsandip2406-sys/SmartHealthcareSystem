using AIService.Services;
using Shared.Library.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class OpenRouterService : IAIService
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;

    public OpenRouterService(IHttpClientFactory factory, IConfiguration config)
    {
        _http = factory.CreateClient();
        _config = config;
    }

    private HttpClient CreateHttpClient()
    {
        var apiKey = _config["OpenRouter:ApiKey"];
        var baseUrl = _config["OpenRouter:BaseUrl"] ?? "https://openrouter.ai/api/v1/";
        var client = new HttpClient
        {
            BaseAddress = new Uri(baseUrl)
        };
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        return client;
    }

    public async Task<string> PatientSummary(string patientInfo, CancellationToken ct = default)
    {
        //HttpClient client = CreateHttpClient();

        var apiKey = _config["OpenRouter:ApiKey"];
        var model = _config["OpenRouter:Model"];

        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        _http.DefaultRequestHeaders.Add("HTTP-Referer", "http://localhost:5000"); // required by OpenRouter
        _http.DefaultRequestHeaders.Add("X-Title", "Healthcare Assistant");
        // 2. Send to OpenRouter API


        var payload = new
        {
            model,
            messages = new[] {
                new { role = "system", content = "You are a medical assistant summarizing patient records." },
                new { role = "user", content = patientInfo }
            }
        };

        var resp = await _http.PostAsync("https://openrouter.ai/api/v1/chat/completions",
            new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"), ct);

        var json = await resp.Content.ReadAsStringAsync(ct);
        using var doc = JsonDocument.Parse(json);
        return doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString()
               ?? "No response";
    }

    public async Task<string> ChatAsync(string userMessage, CancellationToken ct = default)
    {
        var apiKey = _config["OpenRouter:ApiKey"];
        var model = _config["OpenRouter:Model"];
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        _http.DefaultRequestHeaders.Add("HTTP-Referer", "http://localhost:5000"); // required by OpenRouter
        _http.DefaultRequestHeaders.Add("X-Title", "Healthcare Assistant");

        var payload = new
        {
            model,
            messages = new[] {
                new { role = "system", content = "You are a helpful assistant." },
                new { role = "user", content = userMessage } 
            }
        };

        var resp = await _http.PostAsync("https://openrouter.ai/api/v1/chat/completions",
            new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"), ct);

        var json = await resp.Content.ReadAsStringAsync(ct);
        using var doc = JsonDocument.Parse(json);
        return doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString()
               ?? "No response";


        //HttpClient client = CreateHttpClient();
        //var payload = new
        //{
        //    model = "openai/gpt-4o-mini", // free & fast model
        //    messages = new[]
        //            {
        //        new { role = "system", content = "You are a helpful assistant." },
        //        new { role = "user", content = userMessage }
        //    }
        //};

        //var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        //var response = await client.PostAsync("chat/completions", content);

        //response.EnsureSuccessStatusCode();
        //var responseString = await response.Content.ReadAsStringAsync();

        //using var doc = JsonDocument.Parse(responseString);
        //return doc.RootElement
        //          .GetProperty("choices")[0]
        //          .GetProperty("message")
        //          .GetProperty("content")
        //          .GetString() ?? "";
    }

    public async Task<string> SummarizePatientAsync(int patientId, CancellationToken ct = default)
    {
        return await ChatAsync($"Summarize patient with ID {patientId} from our records", ct);
    }
}
