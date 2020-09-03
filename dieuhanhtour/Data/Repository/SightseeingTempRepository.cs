using dieuhanhtour.Data.Interfaces;
using dieuhanhtour.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Repository
{
    public class SightseeingTempRepository : Repository<SightseeingTemp>, ISightseeingTempRepository
    {
        public SightseeingTempRepository(qltourContext context) : base(context)
        {
        }

        public List<SightseeingTemp> GetByCodeAndStt(string code, int stt)
        {
            return _context.SightseeingTemp.Where(x => x.Code == code && x.Stt == stt).ToList();
        }

        public SightseeingTemp GetByIdSightseesing(decimal id)
        {
            return _context.SightseeingTemp.Where(x => x.Id == id).FirstOrDefault();
        }
    }
}
