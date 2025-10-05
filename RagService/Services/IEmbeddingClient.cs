namespace RagService.API.Services
{
    public interface IEmbeddingClient
    {
        Task<float[]> GetEmbeddingAsync(string Input, CancellationToken ct);
        Task<string> GetCompletionAsync(string prompt, CancellationToken ct);
    }
}
