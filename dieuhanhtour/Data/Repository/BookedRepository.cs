using dieuhanhtour.Data.Interfaces;
using dieuhanhtour.Data.Model;
using dieuhanhtour.Data.Utilities;
using dieuhanhtour.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Repository
{
    public class BookedRepository:Repository<Booked>,IBookedRepository
    {
        public BookedRepository(qltourContext context) : base(context)
        {
        }
        GenerateId generateId = new GenerateId();
        public BookedViewModel getBookingBySupplier(string sgtcode, string supplierid)
        {
            var parammeter = new SqlParameter[]
             {
                    new SqlParameter("@sgtcode",sgtcode),
                    new SqlParameter("@supplierId",supplierid)
             };
            try
            {
                return _context.BookedViewModels.FromSql("spGetBookingBySupplier @sgtcode,@supplierId ", parammeter).SingleOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public int getNextBookingTime(string sgtcode, string supplierId)
        {
            try
            {
                int nextTime = _context.Booked.Where(x => x.Sgtcode == sgtcode && x.Supplierid == supplierId).OrderByDescending(x => x.Times).Take(1).SingleOrDefault().Times;
                return nextTime + 1;
            }
            catch
            {
                return 1;
            }
        }

        public List<SupplierByCode> listSupplierByCode(string sgtcode, string chinhanh)
        {
            var parammeter = new SqlParameter[]
             {
                    new SqlParameter("@sgtcode",sgtcode),                   
                    new SqlParameter("@chinhanh",chinhanh)
             };
            try
            {
                return _context.SupplierByCodes.FromSql("spListSupplierBySgtcode @sgtcode,@chinhanh ", parammeter).ToList();
            }
            catch
            {
                return null; 
            }
        }
        public string lastBooking()
        {
            try
            {
                var booking = _context.Booked.Where(x => x.Booking.Substring(6, 4) == System.DateTime.Now.Year.ToString()).Take(1).OrderByDescending(x => x.Idbooking).FirstOrDefault().Booking;
                return booking.Substring(0,6);
            }
            catch
            {
                return "";
            }
        }

        public string nextBooking()
        {
            var newBooking = generateId.NextId(lastBooking(), "", "000001") + System.DateTime.Now.Year.ToString();
            return newBooking;
           
        }
    }
}
