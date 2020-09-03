using dieuhanhtour.Data.Interfaces;
using dieuhanhtour.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Repository
{
    public class LoaixeRepository : Repository<DsLoaixe>, ILoaixeRepository
    {
        public LoaixeRepository(qltourContext context) : base(context)
        {
        }

        public IEnumerable<DsLoaixe> ListLoaixe()
        {
            return _context.DsLoaixe.FromSql("spListLoaixe");
        }
    }
}
