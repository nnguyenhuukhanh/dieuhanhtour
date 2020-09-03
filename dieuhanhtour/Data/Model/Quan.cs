using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Model
{
    public class Quan
    {
        [Key]
        public int Id { get; set; }
        public string Tentp { get; set; }
        public string Tenquan { get; set; }
    }
}
