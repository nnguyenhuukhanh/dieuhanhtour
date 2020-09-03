using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.ViewModel
{
    public class RoomniteViewModel
    {
        [Key]
        public long Stt { get; set; }

        public string thanhpho { get; set; }

        public string tenkhachsan { get; set; }


        public int sgl { get; set; }
        public int sksgl { get; set; }
        public decimal phivndsgl { get; set; }
        public decimal phiusdsgl { get; set; }
        public int dbl { get; set; }
        public int skdbl { get; set; }
        public decimal phivnddbl { get; set; }
        public decimal phiusddbl { get; set; }
        public int twn { get; set; }
        public int sktwn { get; set; }
        public decimal phivndtwn { get; set; }
        public decimal phiusdtwn { get; set; }
        public int tpl { get; set; }
        public int sktpl { get; set; }
        public decimal phivndtpl { get; set; }
        public decimal phiusdtpl { get; set; }
        //Tong roomnite
        public int tongphong { get; set; }
        // Tong so khach
        public int tongsk { get; set; }
        // tong vnd
        public decimal tongvnd { get; set; }
        // tong usd
        public decimal tongusd { get; set; }
    }
}
