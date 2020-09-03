using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Model
{
    public class Chiphikhac
    {
        [Key]
        public decimal idorthercost { get; set; }
        public string sgtcode { get; set; }
       
        public int fromdate { get; set; }
       
        public int todate { get; set; }
        public string srvtype { get; set; }
        public string srvcode { get; set; }
        public string tour_item { get; set; }
        public int quantity { get; set; }
        public decimal unitprice { get; set; }
        public int km { get; set; }
        public int guidedays { get; set; }
        public decimal amount { get; set; }
        public bool debit { get; set; }
        public bool credit { get; set; }
        public string currency { get; set; }
        
        public int vatin { get; set; }
       
        public int vatout { get; set; }
        public decimal srvprofit { get; set; }
        public string srvnode { get; set; }
        public string logfile { get; set; }
        public string chinhanh { get; set; }
        public bool del { get; set; }
    }
}
