﻿using dieuhanhtour.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Interfaces
{
    public interface IDieuxeRepository:IRepository<Dieuxe>
    {
        IEnumerable<Dieuxe> ListXe(string code);
        int newSttxe(string code);
    }
}
