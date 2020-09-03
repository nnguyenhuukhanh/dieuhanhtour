using dieuhanhtour.Data.Model;
using dieuhanhtour.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Interfaces
{
    public interface IHuongdanRepository:IRepository<Huongdan>
    {
        IEnumerable<Huongdan> ListHuongdan(string code);
        int newStthd(string code);

       
    }
}
