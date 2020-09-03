using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.ViewModel
{
    public class KhachVMBViewModel
    {
        [Key]
        public decimal Idkhach { get; set; }
        public string vmb { get; set; }
    }
}
