using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Model
{
    public class Tournode
    {
        [Key]
        [Column(TypeName = "varchar(17)")]
        public string Sgtcode { get; set; }
        public string Headernode { get; set; }
        public string Footernode { get; set; }
    }
}
