using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Model
{
    public class Dieuxe
    {
        [Key]
        public decimal Idxe { get; set; }
        public string Sgtcode { get; set; }
        public int Sttxe { get; set; }
        public DateTime? Ngaydon { get; set; }
        public string Giodon { get; set; }
        public string Diemdon { get; set; }
        public DateTime? Denngay { get; set; }
        public int  Sokhach { get; set; }
        public string Loaixe { get; set; }
        public string Soxe { get; set; }
        public string Laixe { get; set; }
        public string Dienthoai { get; set; }
        public string Ghichu { get; set; }
        public int Km { get; set; }
        public decimal Dongiakm { get; set; }
        public decimal Kmnl { get; set; }
        public string SupplierId { get; set; }
        public string Lotrinh { get; set; }
        public decimal Chiphi { get; set; }
        public string chinhanh { get; set; }
        public string Logfile { get; set; }
        public bool del { get; set; }
        public string YeuCauXe { get; set; }
    }
}
