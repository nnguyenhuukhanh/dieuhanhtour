using dieuhanhtour.Data.Model;
using dieuhanhtour.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Interfaces
{
    public interface IBookedRepository: IRepository<Booked>
    {
        List<SupplierByCode> listSupplierByCode(string sgtcode, string chinhanh);
        BookedViewModel getBookingBySupplier(string sgtcode, string supplierid);
        int getNextBookingTime(string sgtcode, string supplierId);
        string nextBooking();
    }
}
