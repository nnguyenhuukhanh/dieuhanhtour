using dieuhanhtour.Data.Model;
using dieuhanhtour.Data.Repository;
using dieuhanhtour.ViewModel;
using System.Collections.Generic;
using X.PagedList;

namespace dieuhanhtour.Data.Interfaces
{
    public interface ICompanyRepository : IRepository<Company>
    {
        Company GetCompanyById(string id);
        IEnumerable<Company> ListCompany();
        IEnumerable<Company> ListCompanyKhachdoanND(string chinhanh);
        IPagedList<Company> ListCompany(string searchString, int? page);
        string nextCompanyCode();
        string lastCompanyCode();
      

    }
}
