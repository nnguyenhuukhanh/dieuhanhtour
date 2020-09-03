using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Model
{
    public class Hoteltemp
    {
        [Key]
        public decimal Id { get; set; }
        public string Code { get; set; }
        public int stt { get; set; }
        public int sgl { get; set; }
        public int sglpax { get; set; }
        public decimal sglcost { get; set; }
      //  public int sglfoc { get; set; }
        public int extsgl { get; set; }
        public decimal extsglcost { get; set; }
        public int dbl { get; set; }
        public int dblpax { get; set; }
        public decimal dblcost { get; set; }
        //public int dblfoc { get; set; }
        public int extdbl { get; set; }
        public decimal extdblcost { get; set; }
        public int twn { get; set; }
        public int twnpax { get; set; }
        public decimal twncost { get; set; }
       // public int twnfoc { get; set; }
        public int exttwn { get; set; }
        public decimal exttwncost { get; set; }
        public int homestay { get; set; }
        public int homestaypax { get; set; }
        public decimal homestaycost { get; set; }
       // public int tplfoc { get; set; }
        public string homestaynote { get; set; }
        public int oth { get; set; }
        public int othpax { get; set; }
        public decimal othcost { get; set; }
        public string othtype { get; set; }
        public string currency { get; set; }
        public string note { get; set; }
    }
}
