using dieuhanhtour.Data.Model;
using System.Collections.Generic;
using X.PagedList;

namespace dieuhanhtour.Data.Interfaces
{
    public interface ITourinfRepository:IRepository<Tourinf>
    {
        IPagedList<Tourinf> ListTour(string searchString,string chinhanh,string dieuhanh,bool khachle,bool khachdoan,string fromDate,string toDate, int? page);
        string newSgtcode(System.DateTime batdau,string chinhanh,string macode);
        List<Tourinf> loadTourbyChinhanh(string chinhanh);

        IPagedList<Tourinf> ListTourNoOperator( string chinhanh,bool khachle,bool khachdoan, int? page);
    }
}
