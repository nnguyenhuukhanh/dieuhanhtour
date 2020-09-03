using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dieuhanhtour.Data.Model
{
    public class Dmdaily
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Daily { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Tendaily { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Diachi { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Dienthoai { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Fax { get; set; }
        [Column(TypeName = "varchar(3)")]
        public string Macn { get; set; }       
        //[ForeignKey("Macn")]
        //public virtual Dmchinhanh Dmchinhanh { get; set; }
        public bool Trangthai { get; set; }
    }
}
