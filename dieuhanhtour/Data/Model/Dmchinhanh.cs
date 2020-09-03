using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Model
{
    public class Dmchinhanh
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar(3)")]
        public string Macn { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Tencn { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Diachi { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Thanhpho { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Dienthoai { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string Fax { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string Masothue { get; set; }
        public bool Trangthai { get; set; }
    }
}
