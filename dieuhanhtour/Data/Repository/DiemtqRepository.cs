using dieuhanhtour.Data.Interfaces;
using dieuhanhtour.Data.Model;
using dieuhanhtour.Data.Utilities;
using dieuhanhtour.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace dieuhanhtour.Data.Repository
{
    public class DiemtqRepository : Repository<Dmdiemtq>, IDiemtqRepository
    {
        public DiemtqRepository(qltourContext context) : base(context)
        {
        }

        public IPagedList<Dmdiemtq> GetDiemtq(string searchString, int? page)
        {
            if (page.HasValue && page < 1)
                return null;
            var list = _context.Dmdiemtq.AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
                list = list.Where(x => x.Code.Contains(searchString) || x.Diemtq.Contains(searchString) || x.Tinhtp.Contains(searchString) || x.Giave.ToString().Contains(searchString));
            var count = list.Count();
            const int pageSize = 10;
            var listPaged = list.ToPagedList(page ?? 1, pageSize);

            // return a 404 if user browses to pages beyond last page. special case first page if no items exist
            if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
                return null;

            return listPaged;
        }

        public string newCode(string matinh)
        {
            GenerateId newId = new GenerateId();
            return newId.NextId(lastCode(matinh), matinh, "01");
        }
        public string lastCode(string matinh)
        {
            try
            {
                //return _context.Tourtemplate.OrderByDescending(x => x.Id).Take(1).SingleOrDefault().Id;
                return _context.Dmdiemtq.Where(x => x.Code.Substring(0,3 ) == matinh).OrderByDescending(x => x.Code).Take(1).SingleOrDefault().Code;
            }
            catch { return ""; }
        }

        public IQueryable<Dmdiemtq> getAllDiemtq()
        {
            //var a = _context.Dmdiemtq.AsQueryable();
            //int b = a.Count();
            return _context.Dmdiemtq.AsQueryable() ;
        }

        public List<Dmdiemtq> GetLstDiemtq()
        {
            return _context.Dmdiemtq.Where(x=> x.Diemtq != null && x.Diemtq.Length>0 ).ToList();
        }

        public List<vDmdiemtq> GetLstDiemtqTinh()
        {
            return _context.vDmdiemtq.Where(x => x.Diemtq != null && x.Diemtq.Length > 0).ToList();
        }
    }
}
