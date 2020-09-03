using dieuhanhtour.Data.Interfaces;
using dieuhanhtour.Data.Model;
using dieuhanhtour.Data.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using System.Data.SqlClient;

namespace dieuhanhtour.Data.Repository
{
    public class TourinfRepository : Repository<Tourinf>, ITourinfRepository
    {
        public TourinfRepository(qltourContext context) : base(context)
        {
        }
        GenerateId generateId = new GenerateId();
        public IPagedList<Tourinf> ListTour(string searchString, string chinhanh,string dieuhanh,bool khachle,bool khachdoan, string fromDate, string toDate, int? page)
        {
            if (page.HasValue && page < 1)
                return null;

            // var list = _context.Tourinf.AsQueryable();
            var parameter = new SqlParameter[]
               {
                    new SqlParameter("@chinhanh",chinhanh) ,
                    new SqlParameter("@dieuhanh",dieuhanh),
                    new SqlParameter("@khachle",khachle),
                    new SqlParameter("@khachdoan",khachdoan)
               };
            List<Tourinf> list = _context.Tourinf.FromSql("spListTour @chinhanh, @dieuhanh, @khachle,@khachdoan", parameter).ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                
                if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                {
                    DateTime dtTungay = DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime dtDenngay = DateTime.ParseExact(toDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);                    
                    list = list.Where(x => x.arr >= dtTungay && x.arr <= dtDenngay && (x.sgtcode.Contains(searchString) || x.reference.Contains(searchString) ||  x.concernto.Contains(searchString) || x.operators.Contains(searchString))).OrderBy(x => x.arr).ToList();
                }
                else
                {
                    if (searchString.Contains("-"))
                    {
                        List<string> search = searchString.Split('-').ToList();
                        if (search.Count() == 2)
                        {
                            list = list.Where(x => x.sgtcode.Substring(0, 6).Contains(search[0]) && x.sgtcode.Substring(12, 5).Contains(search[1])).OrderBy(x=>x.arr).ToList();
                        }
                        else
                        {
                            list = list.Where(x => x.sgtcode.Contains(searchString)||    x.reference.Contains(searchString) || x.concernto.Contains(searchString) ||  x.operators.Contains(searchString)).OrderBy(x => x.arr).ToList();
                        }
                    }
                    else
                    {
                        list = list.Where(x => x.reference.Contains(searchString) || x.concernto.Contains(searchString) || x.operators.Contains(searchString)).OrderBy(x => x.arr).ToList();
                    }
                   
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                {
                    DateTime dtTungay = DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime dtDenngay = DateTime.ParseExact(toDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    list = list.Where(x => x.arr >= dtTungay && x.arr <= dtDenngay).OrderBy(x => x.arr).ToList();
                }
                else
                {
                    list = list.Where(x => x.arr >= DateTime.Now.AddMonths(-6)).OrderBy(x => x.arr).ToList();

                }
            }
            
            const int pageSize = 10;
            var listPaged = list.ToPagedList(page ?? 1, pageSize);
           

            // return a 404 if user browses to pages beyond last page. special case first page if no items exist
            if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
                return null;

            return listPaged;
            ////}
            /****** nếu user nhập tìm  tên khách  *******/
            //else
            //{

            //}
        }
        public IPagedList<Tourinf> ListTourNoOperator(string chinhanh,bool khachle,bool khachdoan, int? page)
        {
            if (page.HasValue && page < 1)
                return null;
            var parameter = new SqlParameter[]
             {
                    new SqlParameter("@chinhanh",chinhanh) ,
                    new SqlParameter("@khachle",khachle),
                    new SqlParameter("@khachdoan",khachdoan)
             };
            var list = _context.Tourinf.FromSql("spListTourNoOperator @chinhanh,@khachle,@khachdoan", parameter);
            const int pageSize = 10;
            var listPaged = list.OrderBy(x=>x.arr).ToPagedList(page ?? 1, pageSize);

            // return a 404 if user browses to pages beyond last page. special case first page if no items exist
            if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
                return null;

            return listPaged;
        }

        public string newSgtcode(DateTime batdau, string chinhanh, string macode)
        {

            switch (chinhanh)
            {
                case "STS":
                    chinhanh = "SGT";
                    break;
                default:
                    break;
            }
            var newCode =   generateId.NextId(lastCode(batdau, chinhanh, macode), chinhanh + macode + "-" + batdau.Year.ToString() + "-", "00001");
            return newCode;
        }
        public string lastCode(DateTime batdau, string chinhanh, string macode)
        {
            try
            {
                //switch (chinhanh)
                //{
                //    case "STS":
                //        chinhanh = "SGT";
                //        break;
                //    default:
                //        chinhanh = "SGT";
                //        break;
                //}
                var code = _context.Tourinf.Where(x => x.sgtcode.Substring(0, 12) == chinhanh + macode + "-" + batdau.Year.ToString() + "-").Take(1).OrderByDescending(x => x.sgtcode).FirstOrDefault().sgtcode;
                return code;
            }
            catch
            {
                return "";
            }
        }

        public List<Tourinf> loadTourbyChinhanh(string chinhanh)
        {
            throw new NotImplementedException();
        }

       
    }
}
