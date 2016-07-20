



using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Scheduler.Data.Abstract
{
    public interface IScheduleRepository : IEntityBaseRepository<Model.Entities.Schedule> { }

    public interface IUserRepository : IEntityBaseRepository<Model.Entities.User> { }

    public interface IAttendeeRepository : IEntityBaseRepository<Model.Entities.Attendee> { }
}

