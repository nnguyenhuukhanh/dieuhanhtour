using dieuhanhtour.Data.Interfaces;
using dieuhanhtour.Data.Model;
using dieuhanhtour.Data.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Repository
{
    public class LoginRepository : Repository<LoginModel>, ILoginRepository
    {
        public LoginRepository(qltourContext context) : base(context)
        {
        }

        public int changepass(string username, string newpass)
        {
            var parammeter = new SqlParameter[]
             {
                    new SqlParameter("@username",username),
                    new SqlParameter("@newpass",newpass)
             };

            try
            {
               return  _context.Database.ExecuteSqlCommand("spChangepass @username, @newpass", parammeter);  
            }
            catch
            {
                throw;
            }
        }

        public LoginModel login(string username, string password,string mact)
        {
           var parammeter = new SqlParameter[]
          {
                new SqlParameter("@username",username),
                new SqlParameter("@mact",mact)
          };

            var result = _context.LoginModel.FromSql("spLogin @username, @mact", parammeter).SingleOrDefault();
            if (result == null)
            {
                return null;
            }
            else
            {
                return result;               
            }
        }
    }
}
