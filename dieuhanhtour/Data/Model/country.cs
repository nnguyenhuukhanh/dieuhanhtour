using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Model
{
    public class country
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Codealpha { get; set; }
        public string Nation { get; set; }
        public string Natione { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập mã điện thoại")]
        public string TelCode { get; set; }
        public string Continent { get; set; }
        public string Territory { get; set; }

    }
}
