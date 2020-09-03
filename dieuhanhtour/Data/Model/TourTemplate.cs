using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Model
{
    public class TourTemplate
    {
        [Key]
        [Column(TypeName = "varchar(5)")]
        public string Code { get; set; }
        public string Tourkind { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        [Display(Name ="Tên tour")]
        [Required(ErrorMessage ="Vui lòng nhập tên tour")]
        public string Tentour { get; set; }
        [Display(Name ="Tuyến tham quan")]
        public string   Tuyentq { get; set; }
       
        [Column(TypeName = "nvarchar(50)")]
        [Display(Name ="Chủ đề tour")]
        public string  Chudetour { get; set; }
        [Display(Name ="Số ngày")]
        public int Songay { get; set; }
        [Display(Name ="Chi nhánh")]
        public string Chinhanh { get; set; }
        public string  nguoitao { get; set; }
    }
}
