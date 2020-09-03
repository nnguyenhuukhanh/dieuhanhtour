using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.ViewModel
{
    public class DoanbatdauViewModel
    {
        [Key]
        public long stt { get; set; }
        public string tenphong { get; set; }
        public string sgtcode { get; set; }
        public int sk { get; set; }
        public DateTime batdau { get; set; }
        public DateTime ketthuc { get; set; }
        public string lotrinh { get; set; }
        public decimal  doanhthu { get; set; }
    }
}
