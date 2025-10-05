using RagService.API.Models;
namespace RagService.Services;
public interface IDocumentStore
{
    Task<int> InsertDocumentAsync(string sourceId, string sourceType, string content, float[] embedding, CancellationToken ct = default);
    Task<IEnumerable<DocumentDto>> GetAllDocumentsAsync(CancellationToken ct = default); // for demo
    Task<DocumentDto?> GetDocumentByIdAsync(int id, CancellationToken ct = default);

    Task<int> InsertNoteAsync(int patientId, string note, float[] embedding);
    Task<IEnumerable<PatientNote>> GetAllNotesAsync(int patientId);
}
