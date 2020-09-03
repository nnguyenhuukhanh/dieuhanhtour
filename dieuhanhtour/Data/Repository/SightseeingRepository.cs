using dieuhanhtour.Data.Interfaces;
using dieuhanhtour.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Repository
{
    public class SightseeingRepository : Repository<Sightseeing>, ISightseeingRepository
    {
        public SightseeingRepository(qltourContext context) : base(context)
        {
        }

        public List<Sightseeing> GetLstSightseeingBySgtcode(string sgtcode)
        {
            return _context.Sightseeing.Where(x => x.sgtcode == sgtcode).ToList();
        }

        public List<Sightseeing> GetByCodeAndStt(string code, int stt)
        {
            return _context.Sightseeing.Where(x => x.sgtcode == code && x.Stt == stt).ToList();
        }

        public Sightseeing GetByIdSightseesing(decimal id)
        {
            return _context.Sightseeing.Where(x => x.Id == id).FirstOrDefault();
        }

        public Sightseeing GetSightSeeingByCodeAndStt(string code, int stt)
        {
            //chi lay cac dong chua co
            return _context.Sightseeing.Where(x => x.sgtcode == code && x.Stt == stt ).FirstOrDefault();
        }
    }
}
