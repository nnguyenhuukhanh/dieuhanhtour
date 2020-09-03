using dieuhanhtour.Data.Model;
using dieuhanhtour.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace dieuhanhtour.Data.Interfaces
{
    public interface ISupplierRepository:IRepository<Supplier>
    {
        IPagedList<Supplier> ListSupplier(string searchString, int? page);
        IEnumerable<Supplier> ListSupplier();
        Supplier getSupplierById(string code);
        string nextSupplierCode();
        Supplier UpdateSupplierVM(SupplierViewModel model, string nguoitao);
        Supplier CreateSupplierVM(Supplier model, string nguoitao);
        int DelSupplier(string code);
    }
}
