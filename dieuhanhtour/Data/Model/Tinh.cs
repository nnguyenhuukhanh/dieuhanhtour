using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Model
{
    public class Tinh
    {
        [Key]
        public string Matinh { get; set; }
        public string Tentinh { get; set; }
        public string VungId { get; set; }
        public string Mien { get; set; }
    }
}
