using dieuhanhtour.Data.Interfaces;
using dieuhanhtour.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Repository
{
    public class ChinhanhRepository : Repository<Dmchinhanh>, IChinhanhRepository
    {
        public ChinhanhRepository(qltourContext context) : base(context)
        {
        }

        public IEnumerable<Dmchinhanh> ListChinhanh()
        {
            return _context.Dmchinhanh.FromSql("exec spListChinhanh");
        }
    }
}
