using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Model
{
    public class Dmdiemtq
    {
        [Key]
        [Required(ErrorMessage ="Nhập Code")]
        [MaxLength(6)]
        public string Code { get; set; }
        public string Diemtq { get; set; }
        public string Tinhtp { get; set; }
        public string Thanhpho { get; set; }
        [Required(ErrorMessage = "Nhập giá vé")]
        public Decimal Giave { get; set; }       
        public Decimal Giatreem { get; set; }
        public string Congno { get; set; }
        [Required(ErrorMessage = "Nhập VAT đầu ra")]
        public int Vatvao { get; set; }
        [Required(ErrorMessage = "Nhập VAT đầu ra")]
        public int Vatra { get; set; }
        [Required(ErrorMessage = "Nhập tỉ lệ lãi")]
        public Decimal Tilelai { get; set; }
        public string logfile { get; set; }
    }
}
