using dieuhanhtour.Data.Interfaces;
using dieuhanhtour.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Repository
{
    public class HotelRepository : Repository<Hotel>, IHotelRepository
    {
        public HotelRepository(qltourContext context) : base(context)
        {
        }

        public Hotel GetByCodeAndOrder(string code, int stt)
        {
            return _context.Hotel.Where(x => x.sgtcode == code && x.stt == stt).FirstOrDefault();
        }

        public List<Hotel> GetLstHTLByCodeAndOrder(string code, int stt)
        {
            return _context.Hotel.Where(x => x.sgtcode == code && x.stt == stt).ToList();
        }
    }
}
