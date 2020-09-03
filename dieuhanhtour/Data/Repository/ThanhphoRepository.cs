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
    public class ThanhphoRepository : Repository<Thanhpho>, IThanhphoRepository
    {
        public ThanhphoRepository(qltourContext context) : base(context)
        {
        }

        public int capnhatThanhpho(Thanhpho thanhpho)
        {
            var parammeter = new SqlParameter[]
            {
                    new SqlParameter("@matp",thanhpho.Matp),
                    new SqlParameter("@tentp",thanhpho.Tentp),
                     new SqlParameter("@matinh",thanhpho.Matinh)
            };
            try
            {
                return _context.Database.ExecuteSqlCommand("spUdateThanhphoTrenQltaikhoan @matp, @tentp, @matinh", parammeter);
            }
            catch
            {
                throw;
            }
        }

        public Thanhpho getThanhphoById(string matp)
        {
            var parammeter = new SqlParameter[]
          {
                new SqlParameter("@matp",matp)
          };
            return _context.Thanhpho.FromSql("spGetThanhphoById @matp", parammeter).SingleOrDefault();
        }

        public IEnumerable<Thanhpho> ListThanhpho()
        {
            return _context.Thanhpho.FromSql("select Matp,Tentp,mien as matinh from qltaikhoan.dbo.thanhpho");
        }

        public IEnumerable<Thanhpho> ListThanhphoByTinh(string matinh)
        {
            var parammeter = new SqlParameter[]
           {
                new SqlParameter("@matinh",matinh)                 
           };
            return _context.Thanhpho.FromSql("spListThanhphoByTinh @matinh", parammeter);
        }

        public IEnumerable<vTinh> ListTinh()
        {
            return _context.vTinh.FromSql("select * from qltaikhoan.dbo.vTinh");
        }

        public IEnumerable<Vungmien> ListVung()
        {
            return _context.Vungmien.FromSql("select vungId, tenvung,mien from qltaikhoan.dbo.vungmien");
        }

        public string newMatp(string matinh)
        {
            GenerateId newId = new GenerateId();
            return newId.NextId(lastCode(matinh), matinh, "001");
        }
        public string lastCode(string matinh)
        {
            try
            {
                return ListThanhphoByTinh(matinh).OrderByDescending(x => x.Matp).Take(1).SingleOrDefault().Matp;
            }
            catch { return ""; }
        }
        public int themThanhpho(Thanhpho thanhpho)
        {
            var parammeter = new SqlParameter[]
            {
                    new SqlParameter("@matp",thanhpho.Matp),
                    new SqlParameter("@tentp",thanhpho.Tentp),
                    new SqlParameter("@matinh",thanhpho.Matinh)
            };
            try
            {
                return _context.Database.ExecuteSqlCommand("spAddThanhphoTrenQltaikhoan @matp, @tentp, @matinh", parammeter);
            }
            catch
            {
                throw;
            }
        }
    }
}
