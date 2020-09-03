using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.ViewModel
{
    public class EditTourModel
    {
        [Key]
        public string sgtcode { get; set; }
        public bool khachle { get; set; }
        public string macode { get; set; }
        public string codetemp { get; set; }
        public string companyId { get; set; }
        public int tourkindId { get; set; }
        [Required(ErrorMessage = "Nhập từ ngày")]
        public DateTime? arr { get; set; }
        [Required(ErrorMessage = "Nhập kết thúc")]
        public DateTime? dep { get; set; }
        [Required(ErrorMessage = "(*)")]
        public int pax { get; set; }
        [Required(ErrorMessage = "(*)")]
        public int childern { get; set; }
        public string concernto { get; set; }
        public string operators { get; set; }
        public string departoperator { get; set; }
        public string reference { get; set; }
        public string routing { get; set; }
        public string passtype { get; set; }
        [Required(ErrorMessage = "(***)")]
        public decimal revenue { get; set; }
        public string currency { get; set; }
        [Required(ErrorMessage = "(*)")]
        public decimal rate { get; set; }
        public string visa { get; set; }
        public string chinhanh { get; set; }
    }
}
