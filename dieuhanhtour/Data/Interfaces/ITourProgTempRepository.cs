using dieuhanhtour.Data.Model;
using System.Collections.Generic;

namespace dieuhanhtour.Data.Interfaces
{
    public interface ITourProgTempRepository:IRepository<TourProgTemp>
    {
        IEnumerable<TourProgTemp> ListTourProgTemp(string code);
        int newSttTourProgTemp(string code);
        int newDateTourProg(string code);
    }
}
