using dieuhanhtour.Data.Interfaces;
using dieuhanhtour.Data.Model;
using dieuhanhtour.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Repository
{
    public class HuongdanRepository : Repository<Huongdan>, IHuongdanRepository
    {
        public HuongdanRepository(qltourContext context) : base(context)
        {
        }

        public IEnumerable<Huongdan> ListChuaphanhd(string code)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Huongdan> ListHuongdan(string code)
        {
            return _context.Huongdan.Where(x => x.Sgtcode == code && x.del == false).OrderBy(x => x.Stt);
        }

        

        public int newStthd(string code)
        {
            try
            {
                int a = _context.Huongdan.Where(x => x.Sgtcode == code && x.del == false).OrderByDescending(x => x.Stt).Take(1).SingleOrDefault().Stt;
                return a = a + 1; ;
            }
            catch
            {
                return 1;
            }
        }
    }
}
