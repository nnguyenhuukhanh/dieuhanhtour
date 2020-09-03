using dieuhanhtour.Data.Interfaces;
using dieuhanhtour.Data.Model;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace dieuhanhtour.Data.Repository
{
    public class UserRepository : Repository<Users>, IUserRepository
    {
        public UserRepository(qltourContext context) : base(context)
        {
        }

        public int createUsers_qltaikhoan(string username, string hoten, string password, string maphong,string chinhanh)
        {
            var parammeter = new SqlParameter[]
             {
                    new SqlParameter("@username",username),
                    new SqlParameter("@hoten",hoten),
                    new SqlParameter("@password",password),
                     new SqlParameter("@maphong",maphong),
                    new SqlParameter("@chinhanh",chinhanh),
             };            
            try
            {
                return _context.Database.ExecuteSqlCommand("spTaoNhanvienTrenQltk @username, @hoten, @password,@maphong,@chinhanh ", parammeter);
            }
            catch
            {
                throw;
            }
        }

        public int editUsers_qltaikhoan(string username, string hoten, string password, string maphong, string chinhanh)
        {
            var parammeter = new SqlParameter[]
             {
                    new SqlParameter("@username",username),
                    new SqlParameter("@hoten",hoten),
                    new SqlParameter("@password",password),
                    new SqlParameter("@maphong",maphong),
                    new SqlParameter("@chinhanh",chinhanh),
             };
            try
            {
                return _context.Database.ExecuteSqlCommand("spCapnhatNhanvienTrenQltk @username, @hoten, @password,@maphong,@chinhanh ", parammeter);
            }
            catch
            {
                throw;
            }
        }
    }
}
