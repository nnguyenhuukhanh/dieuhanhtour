using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.ViewModel
{
    public class CPDiemThamQuanViewModel
    {
        [Key]
        public long stt { get; set; }

        public string tentp { get; set; }
        public string diemtq { get; set; }

        public int khachnuocngoai { get; set; }
        public int khachviet { get; set; }
        public decimal cpvnd { get; set; }
    }
}
