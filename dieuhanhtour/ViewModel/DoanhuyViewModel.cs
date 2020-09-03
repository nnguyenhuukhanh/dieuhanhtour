using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.ViewModel
{
    public class DoanhuyViewModel
    {
        [Key]
        public long stt { get; set; }
        public string sgtcode { get; set; }
        public int sk { get; set; }
        public string thoigian { get; set; }
        public string khachsan { get; set; }
        public string nhahang { get; set; }
        public string cano { get; set; }
        public string vannghe { get; set; }
        public string roinuoc { get; set; }
        public string xelua { get; set; }
        public string vanchuyenkhac { get; set; }
        public string huongdan { get; set; }
        public string xe { get; set; }
    }
}
