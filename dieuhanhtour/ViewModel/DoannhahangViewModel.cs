using System;
using System.ComponentModel.DataAnnotations;


namespace dieuhanhtour.ViewModel
{
    public class DoannhahangViewModel
    {
        [Key]
        public long stt { get; set; }
        public string tennh  { get; set; }
        public string sgtcode { get; set; }
        public DateTime ngay { get; set; }
        public string loai { get; set; }
        public int sk { get; set; }
        public int khachan { get; set; }
        public string tieuchuan { get; set; }
        public string ghichu { get; set; }
        public string thitruong { get; set; }
    }
}
