using dieuhanhtour.Data.Model;
using System.Collections.Generic;

namespace dieuhanhtour.Data.Interfaces
{
    public interface ITourprogRepository:IRepository<Tourprog>
    {
        IEnumerable<Tourprog> ListTourProg(string code);
        int newSttTourProg(string code);
        int newDateTourProg(string code);
        int maxDate(string sgtcode);
        IEnumerable<Tourprog> ListTourProgBySupplier(string sgtcode,string supplierid);
    }
}
