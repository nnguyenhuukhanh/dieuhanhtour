using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dieuhanhtour.Data.Model
{
    public class Users
    {
        [Key]
        [Required(ErrorMessage = "Nhập Username")]
        [Remote("UserExists", "Nhanvien", ErrorMessage = "Username đã có")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Nhập Họ tên nv")]
        public string Hoten { get; set; }
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail không đúng")]
        [Required(ErrorMessage = "Nhập Email")]
        public string Email { get; set; }
        //[Required(ErrorMessage = "Nhập Password")]       
        public string Password { get; set; }
        public bool khachle { get; set; }
        public bool khachdoan { get; set; }
        public string Maphong { get; set; }
        public bool Newtour { get; set; }
        public bool Dongtour { get; set; }
        public bool Catalogue { get; set; }
        public bool Booking { get; set; }
        public bool Report { get; set; }
        public bool Showprice { get; set; }
        public bool Print { get; set; }
        public bool Doixe { get; set; }
        public bool Maybay { get; set; }
        public bool Huongdan { get; set; }
        public bool Sales { get; set; }
        public bool Vetq { get; set; }
        public bool Admin { get; set; }
        public bool Active { get; set; }
        [Required(ErrorMessage = "Chọn chi nhánh")]
        public string chinhanh { get; set; }
    }
}
