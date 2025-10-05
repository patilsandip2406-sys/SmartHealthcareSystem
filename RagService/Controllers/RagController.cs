using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RagService.API.Models;
using RagService.API.Services;
using RagService.Services;

namespace RagService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RagAiController : ControllerBase
    {
        private readonly IDocumentStore _store;
        private readonly IRagService _rag;
        private readonly IEmbeddingClient _emb;
        public RagAiController(IDocumentStore store, IRagService rag, IEmbeddingClient emb)
        {
            _store = store;
            _rag = rag;
            _emb = emb;
        }

        [HttpPost("ingest")]
        public async Task<ActionResult> Ingest([FromBody] IngestRequest req, CancellationToken ct)
        {
            var emb = await _emb.GetEmbeddingAsync(req.Content, ct);
            var id = await _store.InsertDocumentAsync(req.SourceId, req.SourceType, req.Content, emb, ct);
            return Ok(new { id });
        }

        [HttpGet("query")]
        public async Task<ActionResult> Query(string q, int k = 4, CancellationToken ct = default)
        {
            var res = await  _rag.QueryAsync(q, k, ct);
            return Ok(res);
        }

        [HttpGet("ragPatientSummary/Id")]
        public async Task<ActionResult> PatientSummary(int id, CancellationToken ct)
        {
            var res = await _rag.PatientSummaryAsync(id, ct);
            return Ok(res);
        }


        [HttpPost("ingestPatientNotes")]
        public async Task<IActionResult> IngestPatientNotes([FromBody] IngestNoteRequest req, CancellationToken ct)
        {
            var emb = await _emb.GetEmbeddingAsync(req.Note, ct);
            var id = await _store.InsertNoteAsync(req.PatientId, req.Note, emb);
            return Ok(new { id });
        }

        [HttpGet("ragPatientNotes/{patientId}")]
        public async Task<IActionResult> PatientNotes(int patientId, [FromQuery] string query)
        {
            var res = await _rag.GetPatientNotesAsync(patientId, query);
            return Ok(res);
        }
    }
}
