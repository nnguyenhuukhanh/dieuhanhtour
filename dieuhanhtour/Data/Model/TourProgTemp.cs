using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Model
{
    public class TourProgTemp
    {
        [Key]
        public decimal Id { get; set; }
        public string Code { get; set; }        
        public int stt { get; set; }
        public int date { get; set; }
        public  string time { get; set; }
        public string srvtype { get; set; }
        public string supplierid { get; set; }
        public string srvcode { get; set; }
        public string tour_item { get; set; }
        public string srvnode { get; set; }
        public string currency { get; set; }
        public string arr { get; set; }
        public string dep { get; set; }
        public string carrier { get; set; }
        public string airtype { get; set; }
        public string pickuptime { get; set; }
        public decimal unitpricea { get; set; }
        public decimal unitpricec { get; set; }
        public int foc { get; set; }
        public string carguide { get; set; }
        public decimal amount { get; set; }
        public bool debit { get; set; }
        [Required(ErrorMessage="*")]
        public int vatin { get; set; }
        [Required(ErrorMessage = "*")]
        public int vatout { get; set; }
        public string chinhanh { get; set; }

    }
}
