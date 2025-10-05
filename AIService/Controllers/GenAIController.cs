using AIService.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class GenAIController : ControllerBase
{
    private readonly IAIService _ai;

    public GenAIController(IAIService ai) => _ai = ai;

    [HttpPost("chat")]
    public async Task<IActionResult> Chat([FromBody] ChatRequest request, CancellationToken ct)
    {
        var result = await _ai.ChatAsync(request.Message, ct);
        return Ok(new { response = result });
    }

    [HttpGet("patient-summary/{patientId:int}")]
    public async Task<IActionResult> PatientSummary(int patientId, CancellationToken ct)
    {
        var result = await _ai.SummarizePatientAsync(patientId, ct);
        return Ok(new { summary = result });
    }
}

public record ChatRequest(string Message);
