using Shared.Library.Models;

namespace AIService.Services
{
    public interface IPatientInfo
    {
        Task<string> PatientDetails(Patient patient, CancellationToken ct = default);
    }
}
