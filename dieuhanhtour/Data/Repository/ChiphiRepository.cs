using dieuhanhtour.Data.Interfaces;
using dieuhanhtour.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Repository
{
    public class ChiphiRepository : Repository<Chiphikhac>, IChiphiRepository
    {
        public ChiphiRepository(qltourContext context) : base(context)
        {
        }

        public List<Chiphikhac> GetlstCP(string code)
        {
            return _context.Chiphikhac.Where(x => x.sgtcode == code && x.del == false).ToList();
        }
    }
}
