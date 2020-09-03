using dieuhanhtour.Data.Interfaces;
using dieuhanhtour.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;


namespace dieuhanhtour.Data.Repository
{
    public class QuocgiaRepository : Repository<Quocgia>, IQuocgiaRepository
    {
        public QuocgiaRepository(qltourContext context) : base(context)
        {
        }

        public IEnumerable<Quocgia> ListQuocgia()
        {
            return _context.Quocgia.FromSql("select * from qltour.dbo.Quocgia");
        }
    }
}
