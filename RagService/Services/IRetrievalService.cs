using RagService.API.Models;

namespace RagService.API.Services
{
    public interface IRetrievalService
    {
        Task<List<(DocumentDto doc, float score)>> RetrieveTopKAsync(string query, int k = 5, CancellationToken ct = default);
    }
}
