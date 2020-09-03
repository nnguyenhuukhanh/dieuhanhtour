
using System.ComponentModel.DataAnnotations;

namespace dieuhanhtour.ViewModel
{
    public class ConfirmdichvuViewModel
    {
        [Key]
        public long stt { get; set; }
        public string donvi { get; set; }
        public string noidung { get; set; }
        public string trangthai { get; set; }
    }
}
