using AIService.Services;
using Infrastructure.Library.DbContexts;
using Microsoft.AspNetCore.Mvc;

namespace AIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientSummaryController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAIService _ai;
        private readonly IPatientInfo _patientInfo;

        public PatientSummaryController(AppDbContext context, IAIService ai, IPatientInfo patientInfo)
        {
            _context = context;
            _ai = ai;
            _patientInfo = patientInfo;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientSummary(int id, CancellationToken ct)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return NotFound();

            string patientInfo = await _patientInfo.PatientDetails(patient);

            var aiSummary = await _ai.PatientSummary(patientInfo.ToString(), ct);
            return Ok(new
            {
                Patient = patient,
                summary = aiSummary
            });
        }
    }
}
