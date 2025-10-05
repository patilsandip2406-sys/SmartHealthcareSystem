namespace AIService.Services
{
    public interface IAIService
    {
        Task<string> ChatAsync(string userMessage, CancellationToken ct = default);
        Task<string> PatientSummary(string patientInfo, CancellationToken ct = default);
        Task<string> SummarizePatientAsync(int patientId, CancellationToken ct = default);

    }
}
