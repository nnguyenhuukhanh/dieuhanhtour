using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Model
{
    public class Sightseeing
    {
        [Key]
        public decimal Id { get; set; }
        public string sgtcode { get; set; }
        public int? Stt { get; set; }
        public string Codedtq { get; set; }
        public string Serial { get; set; }
        public string Debit { get; set; }
        public int? Pax { get; set; }
        public decimal? PaxPrice { get; set; }
        public int? Childern { get; set; }
        public decimal? ChildernPrice { get; set; }
        public decimal? Amount { get; set; }
        public int? Vatin { get; set; }
        public int? Vatout { get; set; }
        public string Chinhanh { get; set; }
        public string Httt { get; set; }
        public string logfile { get; set; }
       // public bool del { get; set; }
    }
}
