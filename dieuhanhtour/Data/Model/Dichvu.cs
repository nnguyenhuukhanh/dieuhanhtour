using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Model
{
    public class Dichvu
    {
        [Key]
        [Required(ErrorMessage ="Nhập Mã DV")]
        [Column(TypeName = "varchar(3)")]
        [Remote("DichvuExists", "Dichvu", ErrorMessage = "Mã DV đã tồn tại")]
        public string Iddichvu { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Tendv { get; set; }
        public bool Trangthai { get; set; }

        //public ICollection<Huydv> Huydv { get; set; }
    }
}
