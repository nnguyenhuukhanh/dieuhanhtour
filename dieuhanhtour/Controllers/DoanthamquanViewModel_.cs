using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.ViewModel
{
    public class DoanthamquanViewModel
    {
        [Key]
        public long stt { get; set; }
        public string diemtp { get; set; }
        public string sgtcode { get; set; }
        public int khachnuocngoai { get; set; }
        public int khachviet { get; set; }
    }
}
