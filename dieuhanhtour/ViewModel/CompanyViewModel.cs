using dieuhanhtour.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.ViewModel
{
    public class CompanyViewModel
    {
        //[Key]
        //public string companyId { get; set; }
        //public string name { get; set; }
        //public string fullname { get; set; }
        //public string nation { get; set; }
        //public string fax { get; set; }
        //public string tel { get; set; }
        //public string address { get; set; }
        //public DateTime? contact { get; set; }
        //public string natione { get; set; }
        //public string headoffice { get; set; }
        //public string msthue { get; set; }
        //public string chinhanh { get; set; }

        public string Referer { get; set; }

        public Company Company { get; set; }

    }
}
