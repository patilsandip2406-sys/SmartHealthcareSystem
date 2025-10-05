using AIService.Services;
using Microsoft.Extensions.Caching.Memory;

public class CachingAIService : IAIService
{
    private readonly IAIService _inner;
    private readonly IMemoryCache _cache;

    public CachingAIService(IAIService inner, IMemoryCache cache)
    {
        _inner = inner;
        _cache = cache;
    }

    public async Task<string> PatientSummary(string patientInfo, CancellationToken ct = default)
    {
        if (_cache.TryGetValue(patientInfo, out string cached))
            return cached;

        var result = await _inner.PatientSummary(patientInfo, ct);
        _cache.Set(patientInfo, result, TimeSpan.FromMinutes(10));
        return result;
    }

    public async Task<string> ChatAsync(string userMessage, CancellationToken ct = default)
    {
        if (_cache.TryGetValue(userMessage, out string cached))
            return cached;

        var result = await _inner.ChatAsync(userMessage, ct);
        _cache.Set(userMessage, result, TimeSpan.FromMinutes(10));
        return result;
    }

    public async Task<string> SummarizePatientAsync(int patientId, CancellationToken ct = default)
    {
        var key = $"patient:{patientId}";
        if (_cache.TryGetValue(key, out string cached))
            return cached;

        var result = await _inner.SummarizePatientAsync(patientId, ct);
        _cache.Set(key, result, TimeSpan.FromMinutes(10));
        return result;
    }
}
