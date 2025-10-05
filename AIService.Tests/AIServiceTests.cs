using AIService.Services;
using Microsoft.Extensions.Caching.Memory;

namespace AIService.Tests
{
    public class AIServiceTests
    {
        [Fact]
        public async Task ChatAsync_ReturnsCachedValue()
        {
            var fake = new FakeAIService();
            var cache = new MemoryCache(new MemoryCacheOptions());
            var svc = new CachingAIService(fake, cache);

            var first = await svc.ChatAsync("Hello");
            var second = await svc.ChatAsync("Hello");
            Assert.Equal("Echo: Hello", first);
        }

        private class FakeAIService : IAIService
        {
            public Task<string> ChatAsync(string userMessage, CancellationToken ct = default) => Task.FromResult("ok"); 
            public Task<string> PatientSummary(string userMessage, CancellationToken ct = default) => Task.FromResult("summary");
            public Task<string> SummarizePatientAsync(int patientId, CancellationToken ct = default) => Task.FromResult("summary");
        }
    }
}