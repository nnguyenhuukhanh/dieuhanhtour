using System;
using System.ComponentModel.DataAnnotations;

namespace dieuhanhtour.ViewModel
{
    public class DoancanoViewModel
    {
        [Key]
        public long stt { get; set; }
        public string tenhang { get; set; }
        public string sgtcode { get; set; }
        public DateTime ngay { get; set; }
        public int sk { get; set; }
        public string noidung { get; set; }
        public string ghichu { get; set; }
        public string thitruong { get; set; }
    }
}
