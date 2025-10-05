using Infrastructure.Library.DbContexts;
using Microsoft.EntityFrameworkCore;
using Shared.Library.Models;

namespace AIService.Services
{
    public class PatientInfoServices : IPatientInfo
    {
        private readonly AppDbContext _context;

        public PatientInfoServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> PatientDetails(Patient patient, CancellationToken ct = default)
        {
            //var patient = await _context.Patients.FindAsync(id);
            var claims = await _context.Claims.FindAsync(patient.Id);

            DateTime today = DateTime.Today;
            int age = today.Year - patient.DOB.Year;

            var patientInfo = $"Summarize the condition of: Patient {patient.Name}, {age} years old, " +
                          $"Claims Amount {claims.Amount}, current status: {claims.Status}.";

            return patientInfo.ToString();
        }
    }
}
