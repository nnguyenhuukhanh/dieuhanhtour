using dieuhanhtour.Data.Interfaces;
using dieuhanhtour.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace dieuhanhtour.Data.Repository
{
    public class PhongbanRepository : Repository<Phongban>, IPhongbanRepository
    {
        public PhongbanRepository(qltourContext context) : base(context)
        {
        }

        public Phongban getPhongbanById(string id)
        {
            var parammeter = new SqlParameter[]
         {
                new SqlParameter("@id",id)
         };

            var result = _context.Phongban.FromSql("spGetPhongbanById @id", parammeter).SingleOrDefault();
            if (result == null)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        public IEnumerable<Phongban> ListPhongban()
        {
            //return _context.Phongban.FromSql("select * from qltour.dbo.phongban where macode is not null");
            return _context.Phongban.Where(x=>x.trangthai==true);
        }

        public IPagedList<Phongban> ListPhongban(string searchString, int? page)
        {
            if (page.HasValue && page < 1)
                return null;
            var list = _context.Phongban.AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
                list = list.Where(x => x.maphong.Contains(searchString) || x.tenphong.Contains(searchString));
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
