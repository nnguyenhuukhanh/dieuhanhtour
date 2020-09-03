using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Model
{
    public class Vungmien
    {
        [Key]
        public string VungId { get; set; }
        public string TenVung { get; set; }
        public string Mien { get; set; }
    }
}
