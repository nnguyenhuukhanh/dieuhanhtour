using dieuhanhtour.Data.Interfaces;
using dieuhanhtour.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Repository
{
    public class TourProgTempRepository : Repository<TourProgTemp>, ITourProgTempRepository
    {
        public TourProgTempRepository(qltourContext context) : base(context)
        {
        }

        public IEnumerable<TourProgTemp> ListTourProgTemp(string code)
        {
            return _context.TourProgTemp.Where(x => x.Code == code).OrderBy(x => x.stt);
        }

        public int newDateTourProg(string code)
        {
            try
            {
                int a = _context.TourProgTemp.Where(x => x.Code == code).OrderByDescending(x => x.date).Take(1).SingleOrDefault().date;
                return a = a + 1;
            }
            catch
            {
                return 1;
            }
        }

        public int newSttTourProgTemp(string code)
        {
            try
            {
                int a = _context.TourProgTemp.Where(x => x.Code == code).OrderByDescending(x => x.stt).Take(1).SingleOrDefault().stt;
                return a = a + 1;
            }
            catch
            {
                return 1;
            }
        }
    }
}
