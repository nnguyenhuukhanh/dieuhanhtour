using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Model
{
    public class Huongdan
    {
        [Key]

        public decimal IdHuongdan { get; set; }
        [Column(TypeName = "varchar(17)")]
        public string Sgtcode { get; set; }
        public int Stt { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime Ngayyeucau { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Tenhd { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string Dienthoai { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string Ngoaingu { get; set; }
        [Column(TypeName = "nvarchar(5)")]
        public string hopdongcty { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? Batdau { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string Batdautai { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? Ketthuc { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string Ketthuctai { get; set; }
        public bool Suottuyen { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Ghichu { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Ndcongviec { get; set; }
        [Column(TypeName = "varchar(3)")]
        public string Loaitien { get; set; }
        public decimal Phidontien { get; set; }
        public decimal Phididoan { get; set; }
        public decimal Traphi { get; set; }
        public string  chinhanh { get; set; }
        public string Logfile { get; set; }
        public bool del { get; set; }
    }
}
