namespace RagService.API.Models;

public record IngestRequest(string SourceId, string SourceType, string Content);
public record QueryResponse(DocumentDto Document, float Score);
public record ChatResponse(string Answer, List<QueryResponse> UsedDocs);
