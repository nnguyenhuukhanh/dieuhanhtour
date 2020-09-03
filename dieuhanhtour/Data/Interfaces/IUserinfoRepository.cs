using dieuhanhtour.Data.Model;
using System.Collections.Generic;

namespace dieuhanhtour.Data.Interfaces
{
    public interface IUserinfoRepository:IRepository<UserInfo>
    {
        UserInfo GetUserByUsername(string username);

        List<UserInfo> GetLstUserInfoByPhong(string maphong,bool khachle,string chinhanh);
        List<UserInfo> dsUser();
       
    }
}
