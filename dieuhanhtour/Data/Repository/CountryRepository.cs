using dieuhanhtour.Data.Interfaces;
using dieuhanhtour.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Repository
{
    public class CountryRepository : Repository<country>, ICountryRepository
    {
        public CountryRepository(qltourContext context) : base(context)
        {
        }
    }
}
