using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Model
{
    public class Nation
    {
        [Key]
        public string Code { get; set; }
        public string CountryName { get; set; }
        public string Telcode { get; set; }
    }
}
