using dieuhanhtour.Data.Interfaces;
using dieuhanhtour.Data.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace dieuhanhtour.Data.Repository
{
    public class UserinfoRepository : Repository<UserInfo>, IUserinfoRepository
    {
        public UserinfoRepository(qltourContext context) : base(context)
        {
        }

        public List<UserInfo> dsUser()
        {
            List<UserInfo> lst = new List<UserInfo>();
            lst = _context.UserInfo.FromSql("select u.*,'' tenphong from qltaikhoan.dbo.users u inner join qltaikhoan.dbo.ApplicationUser a on a.username=u.username and mact='001' ").ToList();
            return lst;
        }

        public List<UserInfo> GetLstUserInfoByPhong(string maphong,bool khachle,string chinhanh)
        {
            List<UserInfo> lst = new List<UserInfo>();
            var parammeter = new SqlParameter[]
           {
                    new SqlParameter("@maphong",maphong),
                     new SqlParameter("@khachle",khachle),
                    new SqlParameter("@chinhanh",chinhanh)
           };

            if (!string.IsNullOrEmpty(maphong))
            {                
                lst=_context.UserInfo.FromSql("spListUserByPhong @maphong,@khachle,@chinhanh", parammeter).ToList();
            }
           
            var empty = new UserInfo
            {
                username = "",
                hoten = "-- Chọn nhân viên --"
            };
            lst.Insert(0, empty);
            return lst;
        }

        public UserInfo GetUserByUsername(string username)
        {
            var parammeter = new SqlParameter[]
            {
                    new SqlParameter("@username",username)
            };

            var result = _context.UserInfo.FromSql("spGetUserByUsername @username", parammeter).SingleOrDefault();
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
