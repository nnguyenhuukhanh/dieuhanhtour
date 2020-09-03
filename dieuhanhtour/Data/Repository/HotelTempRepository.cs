using System.Collections.Generic;
using System.Linq;
using dieuhanhtour.Data.Interfaces;
using dieuhanhtour.Data.Model;
namespace dieuhanhtour.Data.Repository
{
    public class HotelTempRepository : Repository<Hoteltemp>, IHotelTempRepository
    {
        public HotelTempRepository(qltourContext context) : base(context)
        {
        }

        public Hoteltemp GetByCodeAndOrder(string code, int stt)
        {
            return _context.Hoteltemp.Where(x => x.Code == code && x.stt == stt).FirstOrDefault();
        }

        public List<Hoteltemp> GetLstHTLByCodeAndOrder(string code, int stt)
        {
            return _context.Hoteltemp.Where(x => x.Code == code && x.stt == stt).ToList();
        }
    }
}
