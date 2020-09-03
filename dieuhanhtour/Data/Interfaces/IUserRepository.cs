using dieuhanhtour.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Interfaces
{
    public interface IUserRepository:IRepository<Users>
    {
        //IEnumerable<Users> listUser(string searchString);
        int createUsers_qltaikhoan(string username, string hoten, string password,string maphong, string chinhanh);

        int editUsers_qltaikhoan(string username, string hoten, string password, string maphong, string chinhanh);

    }
}
