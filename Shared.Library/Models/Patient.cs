using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Library.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime DOB { get; set; }

    }
}
