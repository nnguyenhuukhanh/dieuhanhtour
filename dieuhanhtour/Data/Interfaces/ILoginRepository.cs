using dieuhanhtour.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Interfaces
{
    public interface ILoginRepository:IRepository<LoginModel>
    {
        LoginModel login(string username, string password,string mact);
        int changepass(string username, string newpass);
    }
}
