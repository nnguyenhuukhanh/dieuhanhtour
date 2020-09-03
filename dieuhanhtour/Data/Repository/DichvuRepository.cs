using dieuhanhtour.Data.Interfaces;
using dieuhanhtour.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Repository
{
    public class DichvuRepository : Repository<Dichvu>, IDichvuRepository
    {
        public DichvuRepository(qltourContext context) : base(context)
        {
        }

        public IEnumerable<Dichvu> ListDichvu_baocao()
        {
            return _context.Dichvu.FromSql("exec splistDichvukhac_baocao");
        }
    }
}
