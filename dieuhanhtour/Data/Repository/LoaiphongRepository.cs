using System.Collections.Generic;
using System.Linq;
using dieuhanhtour.Data.Interfaces;
using dieuhanhtour.Data.Model;


namespace dieuhanhtour.Data.Repository
{
    public class LoaiphongRepository:Repository<DmLoaiphong>,ILoaiphongRepository
    {
        public LoaiphongRepository(qltourContext context) : base(context)
        {
        }

        public List<DmLoaiphong> GetLstDmLoaiPhong()
        {
            return _context.DmLoaiphong.ToList();
        }
    }
}
