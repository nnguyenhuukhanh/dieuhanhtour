using dieuhanhtour.Data.Interfaces;
using dieuhanhtour.Data.Model;
using dieuhanhtour.Data.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace dieuhanhtour.Data.Repository
{
    public class TuyentqRepository : Repository<Tuyentq>, ITuyentqRepository
    {
        public TuyentqRepository(qltourContext context) : base(context)
        {
        }

        public string newCode()
        {
            GenerateId newId = new GenerateId();
            return newId.NextId(lastSerial(), "", "00001");
        }
        public string lastSerial()
        {
            try
            {
                return _context.Tuyentq.OrderByDescending(x => x.Code).Take(1).SingleOrDefault().Code;
            }
            catch { return ""; }
        }

        public IPagedList<Tuyentq> GetTuyentq(string searchString, int? page)
        {
            if (page.HasValue && page < 1)
                return null;
            var list = _context.Tuyentq.AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
                list = list.Where(x => x.Code.Contains(searchString) || x.Tuyen.Contains(searchString));
            var count = list.Count();
            const int pageSize = 10;
            var listPaged = list.ToPagedList(page ?? 1, pageSize);

            // return a 404 if user browses to pages beyond last page. special case first page if no items exist
            if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
                return null;

            return listPaged;
        }
    }
}
