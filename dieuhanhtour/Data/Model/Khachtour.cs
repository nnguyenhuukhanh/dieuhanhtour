using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Model
{
    public class KhachTour
    {
        [Key]
        public decimal IdKhach { get; set; }
        public string sgtcode { get; set; }
        public int stt { get; set; }
        public string makh { get; set; }
        public string hoten { get; set; }
        public DateTime? ngaysinh { get; set; }
        public bool phai { get; set; }
        public string dienthoai { get; set; }
        public string diachi { get; set; }
        public string quoctich { get; set; }        
        public string loaiphong { get; set; }
        public string cmnd { get; set; }
        public string hochieu { get; set; }
        public DateTime?  ngaycaphc { get; set; }
        public DateTime? hieuluchc { get; set; }
        public bool vmb { get; set; }
        public string prn { get; set; }
        public string ghichu { get; set; }
        public bool del { get; set; }
        public string Logfile { get; set; }

    }
}
