using dieuhanhtour.Data.Interfaces;
using dieuhanhtour.Data.Model;

namespace dieuhanhtour.Data.Repository
{
    public class NgoaiteRepository : Repository<Ngoaite>, INgoaiteRepository
    {
        public NgoaiteRepository(qltourContext context) : base(context)
        {
        }
    }
}
