using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.ViewModel
{
    public class SupplierViewModel
    {
        [Key]
        public string Code { get; set; }
        public string Codecn { get; set; }
        [Required(ErrorMessage = "Nhập tên giao dịch")]
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
        [EmailAddress(ErrorMessage = "Email không đúng.")]
        public string Email { get; set; }
        public bool Trangthai { get; set; }
        public string Nguoitao { get; set; }
        public DateTime Ngaytao { get; set; }
        public string Chinhanh { get; set; }
    }
}
