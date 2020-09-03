﻿using dieuhanhtour.Data.Interfaces;
using dieuhanhtour.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Repository
{
    public class PasstypeRepository : Repository<PassType>, IPasstypeRepository
    {
        public PasstypeRepository(qltourContext context) : base(context)
        {
        }
    }
}
