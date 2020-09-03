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
    public class TourTempRepository : Repository<TourTemplate>, ITourTempRepository
    {
        public TourTempRepository(qltourContext context) : base(context)
        {
        }

        public string newTourTempId()
        {
            GenerateId newId = new GenerateId();
            return newId.NextId(lastSerial(), "", "00001");
        }
        public string lastSerial()
        {
            try
            {
                return _context.Tourtemplate.OrderByDescending(x => x.Code).Take(1).SingleOrDefault().Code;
            }
            catch { return ""; }
        }

        public IPagedList<TourTemplate> GetTourtemplate(string searchString,string chinhanh, int? page)
        {
            if (page.HasValue && page < 1)
                return null;
            var list = _context.Tourtemplate.Where(x=>x.Chinhanh==chinhanh).OrderBy(x=>x.Code).AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
                list = list.Where(x => x.Code.Contains(searchString) || x.Tentour.Contains(searchString)|| x.Chudetour.Contains(searchString)).OrderBy(x=>x.Code);
            var count = list.Count();
            const int pageSize = 10;
            var listPaged = list.ToPagedList(page ?? 1, pageSize);

            // return a 404 if user browses to pages beyond last page. special case first page if no items exist
            if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
                return null;

            return listPaged;
        }

        public TourTemplate getTourTempByCode(string code)
        {
            return _context.Tourtemplate.Where(x => x.Code == code).SingleOrDefault();
        }
    }
}
