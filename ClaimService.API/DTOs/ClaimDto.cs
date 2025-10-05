namespace ClaimService.API.DTOs
{
    public class ClaimDto
    {
        public int ClaimId { get; set; }
        public string Diagnosis { get; set; } = string.Empty;
        public string Procedure { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
