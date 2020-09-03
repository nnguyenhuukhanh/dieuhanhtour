using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.ViewModel
{
    public class BookedViewModel
    {
        [Key]
        public string Sgtcode { get; set; }
        public int Times { get; set; }
        [Key]
        public string Supplierid { get; set; }
        public string Booking { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public string Profile { get; set; }
        public string Logfile { get; set; }
        public string Email { get; set; }
    }
}
