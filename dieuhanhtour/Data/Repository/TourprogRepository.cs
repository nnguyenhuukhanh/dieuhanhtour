using dieuhanhtour.Data.Interfaces;
using dieuhanhtour.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Repository
{
    public class TourprogRepository : Repository<Tourprog>, ITourprogRepository
    {
        public TourprogRepository(qltourContext context) : base(context)
        {
        }

       

        public IEnumerable<Tourprog> ListTourProg(string code)
        {
            return _context.Tourprog.Where(x=>x.sgtcode==code).OrderBy(x => x.stt); 
        }

        public IEnumerable<Tourprog> ListTourProgBySupplier(string sgtcode, string supplierid)
        {
            return _context.Tourprog.Where(x => x.sgtcode == sgtcode && x.supplierid == supplierid).ToList();
        }

        public int maxDate(string sgtcode)
        {
            try
            {
                return _context.Tourprog.Where(x => x.sgtcode == sgtcode && x.date > 0).Select(x => x.date).Max();
            }
            catch
            {
                return 0;
            }
        }

        public int newDateTourProg(string code)
        {
            try
            {
                int a = _context.Tourprog.Where(x => x.sgtcode == code).OrderByDescending(x => x.date).Take(1).SingleOrDefault().date;
                return a = a + 1;
            }
            catch
            {
                return 1;
            }
        }

        public int newSttTourProg(string code)
        {
            try
            {
                int a = _context.Tourprog.Where(x => x.sgtcode == code).OrderByDescending(x => x.stt).Take(1).SingleOrDefault().stt;
                return a = a + 1;
            }
            catch
            {
                return 1;
            }
        }
    }
}
