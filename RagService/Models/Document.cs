namespace RagService.API.Models
{
    public record DocumentDto(int Id, string SourceId, string SourceType, string Content, float[]? Embedding, DateTime CreatedAt);
}
