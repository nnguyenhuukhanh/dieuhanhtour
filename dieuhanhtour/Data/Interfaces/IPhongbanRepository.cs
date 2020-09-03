using dieuhanhtour.Data.Model;
using System.Collections.Generic;
using X.PagedList;

namespace dieuhanhtour.Data.Interfaces
{
    public interface IPhongbanRepository: IRepository<Phongban>
    {
        Phongban getPhongbanById(string id);
        IEnumerable<Phongban> ListPhongban();
        IPagedList<Phongban> ListPhongban(string searchString, int? page);
    }
}
