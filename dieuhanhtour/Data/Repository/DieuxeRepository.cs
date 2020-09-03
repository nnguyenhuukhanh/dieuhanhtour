using dieuhanhtour.Data.Interfaces;
using dieuhanhtour.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Repository
{
    public class DieuxeRepository : Repository<Dieuxe>, IDieuxeRepository
    {
        public DieuxeRepository(qltourContext context) : base(context)
        {
        }

        public IEnumerable<Dieuxe> ListXe(string code)
        {
            return _context.Dieuxe.Where(x => x.Sgtcode == code && x.del == false).OrderBy(x => x.Sttxe);
        }

        public int newSttxe(string code)
        {
            try
            {
                int a = _context.Dieuxe.Where(x => x.Sgtcode == code && x.del == false).OrderByDescending(x => x.Sttxe).Take(1).SingleOrDefault().Sttxe;
                return a = a + 1; ;
            }
            catch
            {
                return 1;
            }
        }
    }
}
