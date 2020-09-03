using dieuhanhtour.Data.Interfaces;
using dieuhanhtour.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Repository
{
    public class KhachTourRepository : Repository<KhachTour>, IKhachTourRepository
    {
        public KhachTourRepository(qltourContext context) : base(context)
        {
        }

        public List<KhachTour> ListKhachTour(string code)
        {
            return _context.KhachTour.Where(x=>x.sgtcode==code && x.del==false).ToList();
        }

        public int newStt(string code)
        {
            try
            {
                int a = _context.KhachTour.Where(x => x.sgtcode == code && x.del == false).OrderByDescending(x => x.stt).Take(1).SingleOrDefault().stt;
                return a = a + 1; ;
            }
            catch
            {
                return 1;
            }
        }
    }
}
