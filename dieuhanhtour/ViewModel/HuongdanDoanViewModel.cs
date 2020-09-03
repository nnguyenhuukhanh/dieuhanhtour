using System;
using System.ComponentModel.DataAnnotations;

namespace dieuhanhtour.ViewModel
{
    public class HuongdanDoanViewModel
    {
        [Key]
        public long stt { get; set; }
        public string sgtcode { get; set; }
        public string lotrinh { get; set; }
        public int sk { get; set; }
        public DateTime batdau { get; set; }
        public DateTime ketthuc { get; set; }
        public string tenhd { get; set; }
        public string dienthoai { get; set; }
        public string ngoaingu { get; set; }
        public string ndcongviec { get; set; }
    }
}
