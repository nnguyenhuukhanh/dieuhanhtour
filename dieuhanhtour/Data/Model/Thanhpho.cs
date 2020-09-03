using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Model
{
    public class Thanhpho
    {
        [Key]
        public string Matp { get; set; }       
        public string Tentp { get; set; }
        public string Matinh { get; set; }
    }
}
