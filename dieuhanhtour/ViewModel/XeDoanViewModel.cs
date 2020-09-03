using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.ViewModel
{
    public class XeDoanViewModel
    {
        [Key]
        public long stt { get; set; }
        public string sgtcode { get; set; }
        public string lotrinh { get; set; }
        public int sokhach { get; set; }
        public string loaixe { get; set; }
        public string laixe { get; set; }
        public string soxe { get; set; }
        public string dienthoai { get; set; }
        public DateTime ngaydon { get; set; }
        public DateTime denngay { get; set; }
    }
}
