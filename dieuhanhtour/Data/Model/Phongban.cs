using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Model
{
    public class Phongban
    {
        [Key]
        [MaxLength(5)]
        [Remote("PhongbanExists", "Phongban", ErrorMessage = "Khối/phòng này đã tồn tại")]
        public string maphong { get; set; }
        public string tenphong { get; set; }
        public bool trangthai { get; set; }
        public string macode { get; set; }
    }
}
