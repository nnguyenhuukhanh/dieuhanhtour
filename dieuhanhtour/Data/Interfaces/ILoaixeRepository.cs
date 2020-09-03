using dieuhanhtour.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Interfaces
{
    public interface ILoaixeRepository:IRepository<DsLoaixe>
    {
        IEnumerable<DsLoaixe> ListLoaixe();
    }
}
