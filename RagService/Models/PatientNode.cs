namespace RagService.API.Models
{
    public record PatientNote(int Id, int PatientId, string Note, float[]? Embedding, DateTime CreatedAt);
    public record IngestNoteRequest(int PatientId, string Note);
    public record RagResponse(string Answer, List<PatientNote> UsedNotes);
}
