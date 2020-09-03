using dieuhanhtour.Data.Interfaces;
using dieuhanhtour.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Repository
{
    public class TourTempNoteRepository : Repository<TourTempNote>, ITourTempNoteRepository
    {
        public TourTempNoteRepository(qltourContext context) : base(context)
        {
        }
    }
}
