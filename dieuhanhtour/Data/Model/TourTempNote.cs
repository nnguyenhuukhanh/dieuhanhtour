using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Model
{
    public class TourTempNote
    {
        [Key]
        public string Code { get; set; }
        public string Header { get; set; }
        public string Footer { get; set; }
    }
}
