using dieuhanhtour.Data.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.ViewModel
{
    public class FileUploadViewModel
    {
        public IEnumerable<KhachTour> lstkhach { get; set; }
        public List<IFormFile> files { get; set; }
        public string sgtcode { get; set; }
    }
}
