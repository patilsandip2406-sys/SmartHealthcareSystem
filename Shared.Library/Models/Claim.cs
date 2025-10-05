using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Library.Models
{
    public class Claim
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public decimal Amount { get; set; }
        public string Diagnosis { get; set; } = string.Empty;
        public string Procedure { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
        public Patient Patient { get; set; }
    }
}
