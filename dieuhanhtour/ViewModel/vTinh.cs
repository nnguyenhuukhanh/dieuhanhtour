using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.ViewModel
{
    public class vTinh
    {
        [Key]
        public string Matinh { get; set; }
        public string Tentinh { get; set; }
        public string VungId { get; set; }
        public string TenVung { get; set; }
        public string Mien { get; set; }
        public string TenMien { get; set; }
    }
}
