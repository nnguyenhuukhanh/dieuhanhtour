using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.ViewModel
{
    public class SupplierByCode
    {
        //[Key]
        //public decimal id { get; set; }
        public string sgtcode { get; set; }
        [Key]
        public string code { get; set; }
        public string tengiaodich { get; set; }
        public string name { get; set; }
        public string email { get; set; }
    }
}
