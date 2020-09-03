using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Model
{
    public class Tourprog
    {
        [Key]
        public decimal Id { get; set; }
        public string sgtcode { get; set; }
        public int stt { get; set; }
        public int date { get; set; }
        public string time { get; set; }
        public int pax { get; set; }
        public int childern { get; set; }
        public string srvtype { get; set; }
        public string supplierid { get; set; }
        public string srvcode { get; set; }
        public string tour_item { get; set; }
        public string srvnode { get; set; }
        public string currency { get; set; }
        public string arr { get; set; }
        public string dep { get; set; }
        public string carrier { get; set; }
        public string  airtype { get; set; }
        public string pickuptime { get; set; }
        public decimal unitpricea { get; set; }
        public decimal unitpricec { get; set; }
        public int foc { get; set; }
        public string carguide { get; set; }
        public decimal amount { get; set; }
        public bool debit { get; set; }
        public int vatin { get; set; }
        public int vatout { get; set; }
        public string status { get; set; }
        public string logfile { get; set; }
        // public bool del { get; set; }//add 27/12/2019
        //[Required(ErrorMessage = "Nhập điều hành")]
        public string dieuhanh { get; set; }
        public string chinhanh { get; set; }
        public DateTime? ngaythang { get; set; }
        public DateTime? ngayhuydv { get; set; }
        public string nguoihuydv { get; set; }
        public string lydohuydv { get; set; }

       

        [ForeignKey("sgtcode")]
        public virtual Tourinf Tourinf { get; set; }
    }
}
