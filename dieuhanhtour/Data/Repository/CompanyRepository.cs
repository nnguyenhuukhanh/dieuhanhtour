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
using X.PagedList;

namespace dieuhanhtour.Data.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        public CompanyRepository(qltourContext context) : base(context)
        {
        }      

        public int DelCompany(string code)
        {
            throw new NotImplementedException();
        }

        public Company GetCompanyById(string id)
        {
            throw new NotImplementedException();
        }

        public string lastCompanyCode()
        {
            try
            {
                var last = ListCompany().OrderByDescending(x => x.companyId).Take(1).SingleOrDefault().companyId;

                return last;
            }
            catch { return ""; }
        }

        public IEnumerable<Company> ListCompany()
        {
            return _context.Company.FromSql("select * from qltour.dbo.company");
        }
        public IEnumerable<Company> ListCompanyKhachdoanND(string chinhanh)
        {
            if(chinhanh=="STN")
            {
                chinhanh = "STS";
            }
            var parameter = new SqlParameter[]
               {
                    new SqlParameter("@chinhanh",chinhanh) 
               };
            return _context.Company.FromSql("spListCompanyKhachdoanND @chinhanh",parameter);
        }

        public IPagedList<Company> ListCompany(string searchString, int? page)
        {
            if (page.HasValue && page < 1)
                return null;

            var list = _context.Company.FromSql("select * from qltour.dbo.company");
            if (!string.IsNullOrEmpty(searchString))
                list = list.Where(x => x.name.Contains(searchString) || x.fullname.Contains(searchString));

            var count = list.Count();
            const int pageSize = 10;

            var listPaged = list.ToPagedList(page ?? 1, pageSize);

            if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
                return null;

            return listPaged;
        }

        //public IPagedList<Company> ListCompany(string searchString, int? page)
        //{
        //    if (page.HasValue && page < 1)
        //        return null;
        //    var parammeter = new SqlParameter[]
        //    {
        //        new SqlParameter("@searchString",searchString)
        //    };
        //    var list = _context.Company.FromSql("exec spListCompany @searchString", parammeter).ToList();

        //    const int pageSize = 10;
        //    var listPaged = list.ToPagedList(page ?? 1, pageSize);

        //    if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
        //        return null;

        //    return listPaged;
        //}

        public string nextCompanyCode()
        {
            GenerateId newId = new GenerateId();
            return newId.NextId(lastCompanyCode(), "", "00001");
        }

      
    }
}
