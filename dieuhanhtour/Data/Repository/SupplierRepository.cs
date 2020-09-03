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
    public class SupplierRepository : Repository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(qltourContext context) : base(context)
        {
        }

        public Supplier getSupplierById(string code)
        {
            var parammeter = new SqlParameter[]
           {
                new SqlParameter("@code",code)
           };
            return _context.Supplier.FromSql("spGetSupplierById @code", parammeter).SingleOrDefault();
        }

        public IEnumerable<Supplier> ListSupplier()
        {

            return _context.Supplier.FromSql("select * from supplier");
        }

        public IPagedList<Supplier> ListSupplier(string searchString, int? page)
        {
            if (page.HasValue && page < 1)
                return null;
            var parammeter = new SqlParameter[]
            {
                new SqlParameter("@searchString",searchString)
            };
            var list = _context.Supplier.FromSql("exec spListSupplier @searchString", parammeter).ToList();
            //var list = _context.Supplier.AsQueryable();
            //if (!string.IsNullOrEmpty(searchString))
            //    list = list.Where(x => x.Code.Contains(searchString) || x.Codecn.Contains(searchString) || x.Tengiaodich.Contains(searchString) || x.Tenthuongmai.Contains(searchString));
            ////var count = list.Count();
            const int pageSize = 10;
            var listPaged = list.ToPagedList(page ?? 1, pageSize);

            // return a 404 if user browses to pages beyond last page. special case first page if no items exist
            if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
                return null;

            return listPaged;
        }

        public string nextSupplierCode()
        {
            GenerateId newId = new GenerateId();
            return newId.NextId(lastSupplierCode(), "", "00001");
        }
        public string lastSupplierCode()
        {
            try
            {
                var last = ListSupplier().OrderByDescending(x => x.Code).Take(1).SingleOrDefault().Code;
                //var last = _context.Supplier.FromSql("spLastSupplierCode").SingleOrDefault().Code;
                return last;
            }
            catch { return ""; }
        }

        public Supplier UpdateSupplierVM(SupplierViewModel model, string nguoitao)
        {
            Supplier s = getSupplierById(model.Code);
            s.Codecn = string.IsNullOrEmpty(model.Codecn) ? "" : model.Code.ToUpper();
            s.Tengiaodich = string.IsNullOrEmpty(model.Tengiaodich) ? "" : model.Tengiaodich.ToUpper();
            s.Tenthuongmai = string.IsNullOrEmpty(model.Tenthuongmai) ? "" : model.Tenthuongmai.ToUpper();
            s.Thanhpho =  model.Thanhpho;
            s.Quocgia = model.Quocgia;
            s.Diachi = string.IsNullOrEmpty(model.Diachi) ? "" : model.Diachi;
            s.Dienthoai = string.IsNullOrEmpty(model.Dienthoai)?"":model.Dienthoai;
            s.Fax = string.IsNullOrEmpty(model.Fax)?"":model.Fax;
            s.Masothue = string.IsNullOrEmpty( model.Masothue)?"":model.Masothue;
            s.Nganhnghe = string.IsNullOrEmpty( model.Nganhnghe)?"":model.Nganhnghe;
            s.Nguoilienhe = string.IsNullOrEmpty( model.Nguoilienhe)?"":model.Nguoilienhe;
            s.Website = string.IsNullOrEmpty(model.Website)?"":model.Website;
            s.Email = string.IsNullOrEmpty( model.Email)?"":model.Email;
            s.Trangthai = model.Trangthai;
            s.Nguoitao = nguoitao;
            s.Ngaytao = System.DateTime.Now;
            s.Chinhanh = model.Chinhanh;
            var parammeter = new SqlParameter[]
            {
                new SqlParameter("@code",s.Code),
                new SqlParameter("@codecn",s.Codecn),
                new SqlParameter("@tengiaodich",s.Tengiaodich),
                new SqlParameter("@tenthuongmai",s.Tenthuongmai),
                new SqlParameter("@thanhpho",s.Thanhpho),
                new SqlParameter("@quocgia",s.Quocgia),
                new SqlParameter("@diachi",s.Diachi),
                new SqlParameter("@dienthoai",s.Dienthoai),
                new SqlParameter("@fax",s.Fax),
                new SqlParameter("@masothue",s.Masothue),
                new SqlParameter("@nganhnghe",s.Nganhnghe),
                new SqlParameter("@nguoilienhe",s.Nguoilienhe),
                new SqlParameter("@website",s.Website),
                new SqlParameter("@email",s.Email),
                new SqlParameter("@trangthai",s.Trangthai)
            };
            int result = _context.Database.ExecuteSqlCommand("spUpdateSupplier @code,@codecn,@tengiaodich,@tenthuongmai,@thanhpho,@quocgia,@diachi,@dienthoai,@fax,@masothue,@nganhnghe,@nguoilienhe,@website,@email,@trangthai", parammeter);
            if(result>0)
            {
                return s;
            }
            else
            {
                return null;
            }
        }

        public Supplier CreateSupplierVM(Supplier model, string nguoitao)
        {
            try
            {
                Supplier s = new Supplier();
                s.Code = model.Code;
                s.Codecn = string.IsNullOrEmpty(model.Codecn) ? "" : model.Codecn;
                s.Tengiaodich = string.IsNullOrEmpty(model.Tengiaodich) ? "" : model.Tengiaodich.ToUpper();
                s.Tenthuongmai = string.IsNullOrEmpty(model.Tenthuongmai) ? "" : model.Tenthuongmai.ToUpper();
                s.Thanhpho = model.Thanhpho;
                s.Quocgia = model.Quocgia;
                s.Diachi = string.IsNullOrEmpty(model.Diachi) ? "" : model.Diachi;
                s.Dienthoai = string.IsNullOrEmpty(model.Dienthoai)?"":model.Dienthoai;
                s.Fax = string.IsNullOrEmpty(model.Fax) ? "" : model.Fax;
                s.Masothue = string.IsNullOrEmpty(model.Masothue)?"":model.Masothue;
                s.Nganhnghe = string.IsNullOrEmpty(model.Nganhnghe)?"":model.Nganhnghe;
                s.Nguoilienhe = string.IsNullOrEmpty(model.Nguoilienhe) ? "" : model.Nguoilienhe;
                s.Website = string.IsNullOrEmpty(model.Website) ? "" : model.Website;
                s.Email = string.IsNullOrEmpty(model.Email) ? "" : model.Email;
                s.Trangthai = model.Trangthai;
                s.Nguoitao = nguoitao;
                s.Ngaytao = System.DateTime.Now;
                s.Chinhanh = model.Chinhanh;
                var parammeter = new SqlParameter[]
                {
                    new SqlParameter("@code",s.Code),
                    new SqlParameter("@codecn",s.Codecn),
                    new SqlParameter("@tengiaodich",s.Tengiaodich),
                    new SqlParameter("@tenthuongmai",s.Tenthuongmai),
                    new SqlParameter("@thanhpho",s.Thanhpho),
                    new SqlParameter("@quocgia",s.Quocgia),
                    new SqlParameter("@diachi",s.Diachi),
                    new SqlParameter("@dienthoai",s.Dienthoai),
                    new SqlParameter("@fax",s.Fax),
                    new SqlParameter("@masothue",s.Masothue),
                    new SqlParameter("@nganhnghe",s.Nganhnghe),
                    new SqlParameter("@nguoilienhe",s.Nguoilienhe),
                    new SqlParameter("@website",s.Website),
                    new SqlParameter("@email",s.Email),
                    new SqlParameter("@trangthai",s.Trangthai),
                    new SqlParameter("@nguoitao",nguoitao),
                    new SqlParameter("@chinhanh",s.Chinhanh),
                };
                int result = _context.Database.ExecuteSqlCommand("spCreateSupplier @code,@codecn,@tengiaodich,@tenthuongmai,@thanhpho,@quocgia,@diachi,@dienthoai,@fax,@masothue,@nganhnghe,@nguoilienhe,@website,@email,@trangthai,@nguoitao,@chinhanh", parammeter);
                if (result > 0)
                {
                    return s;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public int DelSupplier(string code)
        {
            var parammeter = new SqlParameter[]
            {
                 new SqlParameter("@code",code)
            };
            int result = _context.Database.ExecuteSqlCommand("spDelSupplier @code", parammeter);
            return result;
        }
        
    }
}
