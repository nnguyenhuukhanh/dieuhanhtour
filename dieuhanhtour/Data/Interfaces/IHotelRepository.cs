using dieuhanhtour.Data.Model;
using System.Collections.Generic;

namespace dieuhanhtour.Data.Interfaces
{
    public interface IHotelRepository:IRepository<Hotel>
    {
        Hotel GetByCodeAndOrder(string code, int stt);
        List<Hotel> GetLstHTLByCodeAndOrder(string code, int stt);
    }
}
