using dieuhanhtour.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.ViewModel
{
    public class BookKS
    {
        public string tenkhachsan { get; set; }
        public string sgtcode { get; set; }
        public string ngaythang { get; set; }
        public string soluong { get; set; }
        public string foc { get; set; }
        public string loaiphong { get; set; }
        public string thongtinhdv { get; set; }
        public List<KhachTour> listkhach { get; set; }
        public string thanhtoan { get; set; }
    }
}
