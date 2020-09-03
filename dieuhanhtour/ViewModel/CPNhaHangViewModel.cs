using System.ComponentModel.DataAnnotations;

namespace dieuhanhtour.ViewModel
{
    public class CPNhaHangViewModel
    {
        [Key]
        public long Stt { get; set; }
        public string thanhpho { get; set; }
        public string tenkhachsan { get; set; }
        public string srvtype { get; set; }
        public int pax { get; set; }
        public decimal cpvnd { get; set; }
        public decimal cpusd { get; set; }
    }
}
