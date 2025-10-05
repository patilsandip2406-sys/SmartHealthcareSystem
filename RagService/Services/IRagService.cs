using RagService.API.Models;

namespace RagService.API.Services
{
    public interface IRagService
    {
        Task<ChatResponse> QueryAsync(string query, int k = 4, CancellationToken ct = default);
        Task<ChatResponse> PatientSummaryAsync(int patientId, CancellationToken ct = default);

        Task<RagResponse> GetPatientNotesAsync(int patientId, string query);
    }
}
