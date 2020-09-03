using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.ViewModel
{
    public class DoanVisaViewModel
    {
        [Key]
        public long stt { get; set; }
        public string sgtcode { get; set; }
        public string thoigian { get; set; }
        public int sk { get; set; }
        public string thitruong { get; set; }
        public string Reference { get; set; }
        public string visa { get; set; }
    }
}
