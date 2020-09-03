using dieuhanhtour.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Interfaces
{
    public interface ISightseeingTempRepository : IRepository<SightseeingTemp>
    {
        List<SightseeingTemp> GetByCodeAndStt(string code, int stt);
        SightseeingTemp GetByIdSightseesing(decimal id);
    }
}
