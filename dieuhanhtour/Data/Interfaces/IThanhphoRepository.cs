using dieuhanhtour.Data.Model;
using dieuhanhtour.ViewModel;
using System.Collections.Generic;

namespace dieuhanhtour.Data.Interfaces
{
    public interface IThanhphoRepository:IRepository<Thanhpho>
    {
        IEnumerable<Thanhpho> ListThanhpho();
        IEnumerable<Vungmien> ListVung();
        IEnumerable<vTinh> ListTinh();
        IEnumerable<Thanhpho> ListThanhphoByTinh(string matinh);
        Thanhpho getThanhphoById(string matp);
        int capnhatThanhpho(Thanhpho thanhpho);
        int themThanhpho(Thanhpho thanhpho);
        string newMatp(string matinh);
    }
}
