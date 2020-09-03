using dieuhanhtour.Data.Model;
using dieuhanhtour.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace dieuhanhtour.Data.Interfaces
{
    public interface IDiemtqRepository:IRepository<Dmdiemtq>
    {
        IQueryable<Dmdiemtq> getAllDiemtq();
        IPagedList<Dmdiemtq> GetDiemtq(string searchString, int? page);
        string newCode(string matinh);

        List<Dmdiemtq> GetLstDiemtq();
        List<vDmdiemtq> GetLstDiemtqTinh();
    }
}
