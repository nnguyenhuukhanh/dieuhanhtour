using dieuhanhtour.Data.Interfaces;
using dieuhanhtour.Data.Model;
using dieuhanhtour.Data.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace dieuhanhtour.Data.Repository
{
    public class TourinfRepository : Repository<Tourinf>, ITourinfRepository
    {
        public TourinfRepository(qltourContext context) : base(context)
        {
        }
        GenerateId generateId = new GenerateId();
        public IPagedList<Tourinf> ListTour(string searchString, string chinhanh, string fromDate, string toDate, int? page)
        {
            if (page.HasValue && page < 1)
                return null;

            var list = _context.Tourinf.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {

                if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                {
                    DateTime dtTungay = DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime dtDenngay = DateTime.ParseExact(toDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    //list = list.Where(x => x.cancel==null && x.arr >= dtTungay && x.arr <= dtDenngay && x.chinhanh.Contains(chinhanh) && (x.sgtcode.Contains(searchString) || x.reference.Contains(searchString) || x.routing.Contains(searchString))).OrderBy(x => x.arr);
                    list = list.Where(x => x.arr >= dtTungay && x.arr <= dtDenngay && x.chinhanh.Contains(chinhanh) && (x.sgtcode.Contains(searchString) || x.reference.Contains(searchString) || x.routing.Contains(searchString))).OrderBy(x => x.arr);

                }
                else
                {
                    //list = list.Where(x => x.cancel == null && x.chinhanh.Contains(chinhanh) && (x.reference.Contains(searchString) || x.routing.Contains(searchString)) || x.sgtcode.Contains(searchString)).OrderBy(x => x.arr);
                    //list = list.Where(x => x.chinhanh.Contains(chinhanh) && (x.reference.Contains(searchString) || x.routing.Contains(searchString)) || x.sgtcode.Contains(searchString)).OrderBy(x => x.arr);
                    list = list.Where(x => x.chinhanh.Contains(chinhanh) || x.reference.Contains(searchString) || x.routing.Contains(searchString) || x.sgtcode.Contains(searchString)).OrderBy(x => x.arr);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                {
                    DateTime dtTungay = DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime dtDenngay = DateTime.ParseExact(toDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    list = list.Where(x => x.arr >= dtTungay && x.arr <= dtDenngay && x.chinhanh.Contains(chinhanh) && (x.reference.Contains(searchString) || x.routing.Contains(searchString))).OrderBy(x => x.arr);

                    //list = list.Where(x => x.cancel == null && x.arr >= dtTungay && x.arr <= dtDenngay && x.chinhanh.Contains(chinhanh) && (x.reference.Contains(searchString) || x.routing.Contains(searchString))).OrderBy(x => x.arr);
                }
                else
                {
                    list = list.Where(x => (x.arr >= System.DateTime.Now.AddMonths(-6))).OrderBy(x => x.arr);
                    //list = list.Where(x => x.cancel == null && (x.arr >= System.DateTime.Now.AddMonths(-6))).OrderBy(x => x.arr);
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
            var newCode = generateId.NextId(lastCode(batdau, chinhanh, macode), chinhanh + macode + "-" + batdau.Year.ToString() + "-", "00001");
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
    }
}
