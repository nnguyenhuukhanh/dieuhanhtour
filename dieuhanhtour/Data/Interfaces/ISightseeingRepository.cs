using dieuhanhtour.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Interfaces
{
    public interface ISightseeingRepository:IRepository<Sightseeing>
    {
        List<Sightseeing> GetLstSightseeingBySgtcode(string sgtcode);

        List<Sightseeing> GetByCodeAndStt(string code, int stt);
        Sightseeing GetByIdSightseesing(decimal id);
        Sightseeing GetSightSeeingByCodeAndStt(string code, int stt);
    }
}
