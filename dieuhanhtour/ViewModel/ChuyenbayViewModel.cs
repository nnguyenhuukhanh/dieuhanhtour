using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.ViewModel
{
    public class ChuyenbayViewModel
    {
        [Key]
        public long stt { get; set; }
        public string sgtcode { get; set; }
        public DateTime ngayden { get; set; }
        public DateTime ngaydi { get; set; }
        public int sk { get; set; }
        public string lotrinh { get; set; }
        public string ghichu { get; set; }
    }
}
