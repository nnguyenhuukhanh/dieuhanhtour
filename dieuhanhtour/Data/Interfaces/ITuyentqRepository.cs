using dieuhanhtour.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace dieuhanhtour.Data.Interfaces
{
    public interface ITuyentqRepository:IRepository<Tuyentq>
    {
        
        IPagedList<Tuyentq> GetTuyentq(string searchString, int? page);
        string newCode();
    }
}
