using dieuhanhtour.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Interfaces
{
    public interface IKhachTourRepository:IRepository<KhachTour>
    {
        List<KhachTour> ListKhachTour(string code);
        int newStt(string code);
    }
}
