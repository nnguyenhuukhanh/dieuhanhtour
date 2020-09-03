
using System.ComponentModel.DataAnnotations;


namespace dieuhanhtour.Data.Model
{
    public class Quocgia
    {
        [Key]
        public string Code { get; set; }
        public string Nation { get; set; }
        public string Natione { get; set; }
        public string Telcode { get; set; }
    }
}
