using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Model
{
    public class Booked
    {
        [Key]
        public decimal Idbooking { get; set; }
       
        public string Sgtcode { get; set; }
        public int Times { get; set; }
        [Required (ErrorMessage ="Vui lòng chọn nhà cung cấp")]
        public string Supplierid { get; set; }
        public string Booking { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public string Profile { get; set; }
        public DateTime? ConfirmDate { get; set; }
        public string Name { get; set; }// nguoi lien he
        public string Logfile { get; set; }
    }
}
