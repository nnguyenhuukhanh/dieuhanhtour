using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.ViewModel
{
    [Table("vDmdiemtq")]
    public class vDmdiemtq
    {
        [Key]
        public string Code { get; set; }
        public string Diemtq { get; set; }
        public string Tinhtp { get; set; }
    }
}
