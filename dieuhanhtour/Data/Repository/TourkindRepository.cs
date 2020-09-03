using dieuhanhtour.Data.Interfaces;
using dieuhanhtour.Data.Model;


namespace dieuhanhtour.Data.Repository
{
    public class TourkindRepository:Repository<Tourkind>,ITourkindRepository
    {
        public TourkindRepository(qltourContext context) : base(context)
        {
        }
    }
}
