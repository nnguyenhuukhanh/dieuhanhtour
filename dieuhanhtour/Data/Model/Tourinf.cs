using System;
using System.ComponentModel.DataAnnotations;

namespace dieuhanhtour.Data.Model
{
    public class Tourinf
    {
        [Key]
        public string sgtcode { get; set; }
        public bool khachle { get; set; }
        public string companyId { get; set; }
        public int tourkindId { get; set; }
        public DateTime arr { get; set; }
        public DateTime dep { get; set; }
        public int pax { get; set; }
        public int childern { get; set; }
        public string concernto { get; set; }
        public string operators { get; set; }
        public string departoperator { get; set; }
        public string departcreate { get; set; }
        public string reference { get; set; }
        public string routing { get; set; }
        public DateTime? cancel { get; set; }
        public string cancelnote { get; set; }
        public string entryport { get; set; }
        public string entryby { get; set; }
        public string note { get; set; }
        public string visa { get; set; }
        public string passtypeId { get; set; }
        public decimal revenue { get; set; }
        public string currency { get; set; }
        public decimal rate { get; set; }
        public DateTime? createtour { get; set; }
        public DateTime? locktour { get; set; }
        public string userlock { get; set; }
        public string chinhanh { get; set; }
        public string chinhanhtao { get; set; }
        public string logfile { get; set; }
        
    }
}
