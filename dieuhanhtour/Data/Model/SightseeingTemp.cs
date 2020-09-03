using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Model
{
    public class SightseeingTemp
    {
        [Key]
        public decimal Id { get; set; }
        public string Code { get; set; }
        public int? Stt { get; set; }
        public string Codedtq { get; set; }
        public string Serial { get; set; }
        public string Debit { get; set; }
        public decimal? PaxPrice { get; set; }
        public decimal? ChildernPrice { get; set; }
        public decimal? Amount { get; set; }
        public int? Vatin { get; set; }
        public int? Vatout { get; set; }
        public decimal? Srvprofit { get; set; }
        public string chinhanh { get; set; }
        public string httt { get; set; }
    }
}
