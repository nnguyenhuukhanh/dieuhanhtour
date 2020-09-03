using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.ViewModel
{
    public class HotelBookingViewModel
    {
        [Key]
        public long stt { get; set; }
        public string tenks { get; set; }
        public string sgtcode { get; set; }
        public DateTime ngay { get; set; }
        public int sk { get; set; }
        public string phong { get; set; }
        public string ghichu { get; set; }
        public string thitruong { get; set; }
        
    }
}
