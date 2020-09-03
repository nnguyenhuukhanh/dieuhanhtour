using dieuhanhtour.Data.Model;
using System;
using X.PagedList;

namespace dieuhanhtour.Data.Interfaces
{
    public interface ITourTempRepository : IRepository<TourTemplate>
    {
        IPagedList<TourTemplate> GetTourtemplate(string searchString,string chinhanh, int? page);
        string newTourTempId();
        TourTemplate getTourTempByCode(string code);
    }
}