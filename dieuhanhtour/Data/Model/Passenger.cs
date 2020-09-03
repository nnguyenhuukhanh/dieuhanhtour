using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Model
{
    public class Passenger
    {
        [Key]
        [Required,MaxLength(17)]
        public string Sgtcode { get; set; }
        [Key]
        [Required, MaxLength(4)]
        //[Column(TypeName = "varchar(4)")]
        public string Stt { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Hoten { get; set; }
        public bool Gioitinh { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string  Quoctich { get; set; }
        public DateTime? Ngaysinh { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string Hochieu { get; set; }
        public DateTime? NgaycapHC { get; set; }
        public DateTime? HieulucHC { get; set; }
        public string Roomtype { get; set; }
        public DateTime Capnhat { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string Loginname { get; set; }
        public string Logfile { get; set; }
    }
}
