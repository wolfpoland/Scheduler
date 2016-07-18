﻿using Scheduler.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheduler.Data.Repositories
{
    public class ScheduleRepository :EntityBaseRepository<Schedule>
    {
        public ScheduleRepository(SchedulerContext context)
            :base(context)
        {

        }
    }
}
