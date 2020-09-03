using System;
using System.ComponentModel.DataAnnotations;


namespace dieuhanhtour.ViewModel
{
    public class DoankhachsanViewModel
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
