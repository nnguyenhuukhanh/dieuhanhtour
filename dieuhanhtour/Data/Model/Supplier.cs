using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Model
{
    public class Supplier
    {
        [Key]
        [Column(TypeName = "varchar(5)")]
        public string Code { get; set; }
        public string Codecn { get; set; }
        public string Tengiaodich { get; set; }
        public string Tenthuongmai { get; set; }
        public string Thanhpho { get; set; }
        public string Quocgia { get; set; }
        public string Diachi { get; set; }
        public string Dienthoai { get; set; }
        public string Fax { get; set; }
        public string Masothue { get; set; }
        public string Nganhnghe { get; set; }
        public string Nguoilienhe { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public bool Trangthai { get; set; }
        public DateTime Ngaytao { get; set; }
        public string Nguoitao { get; set; }
        public string Chinhanh { get; set; }
    }
}
