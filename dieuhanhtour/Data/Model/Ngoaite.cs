using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Model
{
    public class Ngoaite
    {
        [Key]
        public string MaNT { get; set; }
        public string TenNT { get; set; }
    }
}
