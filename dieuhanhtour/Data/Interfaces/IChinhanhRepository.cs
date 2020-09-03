using dieuhanhtour.Data.Model;
using System.Collections.Generic;


namespace dieuhanhtour.Data.Interfaces
{
    public interface IChinhanhRepository: IRepository<Dmchinhanh>
    {
        IEnumerable<Dmchinhanh> ListChinhanh();
    }
}
