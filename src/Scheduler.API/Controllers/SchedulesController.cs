﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Scheduler.Data.Abstract;
using Scheduler.Model.Entities;
using AutoMapper;
using Scheduler.API.ViewModels;
using Scheduler.API.Core;
using Scheduler.API.ViewModels.Validations;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Scheduler.API.Controllers
{
    [Route("api/[controller]")]
    public class SchedulesController : Controller
    {
        private IScheduleRepository _scheduleRepository;
        private IAttendeeRepository _attendeeRepository;
        private IUserRepository _userRepository;
        int page = 1;
        int pageSize = 4;
        public SchedulesController(IScheduleRepository scheduleRepository,
                                    IAttendeeRepository attendeeRepository,
                                    IUserRepository userRepository)
        {
            _scheduleRepository = scheduleRepository;
            _attendeeRepository = attendeeRepository;
            _userRepository = userRepository;
        }

        public IActionResult Get()
        {
            var pagination = Request.Headers["Pagination"];

            if (!string.IsNullOrEmpty(pagination))
            {
                string[] vals = pagination.ToString().Split(',');
                int.TryParse(vals[0], out page);
                int.TryParse(vals[1], out pageSize);
            }

            int currentPage = page;
            int currentPageSize = pageSize;
            var totalSchedules = _scheduleRepository.Count();
            var totalPages = (int)Math.Ceiling((double)totalSchedules / pageSize);

            IEnumerable<Schedule> _schedules = _scheduleRepository
                .AllIncluding(s => s.Creator, s => s.Attendees)
                .OrderBy(s => s.Id)
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .ToList();

            Response.AddPagination(page, pageSize, totalSchedules, totalPages);

            IEnumerable<ScheduleViewModel> _schedulesVM = Mapper.Map<IEnumerable<Schedule>, IEnumerable<ScheduleViewModel>>(_schedules);

            return new OkObjectResult(_schedulesVM);

        }
        [HttpGet("{id}", Name = "GetSchedule")]
        public IActionResult Get(int id)
        {
            Schedule _schedule = _scheduleRepository.GetSingle(s => s.Id == id, s => s.Creator, s => s.Attendees);
            if (_schedule != null)
            {
                ScheduleViewModel _scheduleVM = Mapper.Map<Schedule, ScheduleViewModel>(_schedule);
                return new OkObjectResult(_scheduleVM);
            } else
            {
                return NotFound();
            }

        }
        [HttpGet("{id}/details", Name = "GetScheduleDetails")]
        public IActionResult GetScheduleDetails(int id)
        {
            Schedule _schedule = _scheduleRepository.GetSingle(s => s.Id == id, s => s.Creator, s => s.Attendees);
            if (_schedule != null)
            {
                ScheduleDetailsViewModel _scheduleDetailsVM = Mapper.Map<Schedule, ScheduleDetailsViewModel>(_schedule);
                foreach (var item in _schedule.Attendees)
                {
                    User userDb = _userRepository.GetSingle(item.UserId);
                    _scheduleDetailsVM.Attendees.Add(Mapper.Map<User, UserViewModel>(userDb));
                }
                return new OkObjectResult(_scheduleDetailsVM);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost]
        public IActionResult Create([FromBody]ScheduleViewModel schedule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Schedule newSchudle = Mapper.Map<ScheduleViewModel, Schedule>(schedule);
            newSchudle.DateCreated = DateTime.Now;
            _scheduleRepository.Add(newSchudle);
            _scheduleRepository.Commit();
            schedule = Mapper.Map<Schedule, ScheduleViewModel>(newSchudle);
            CreatedAtRouteResult result = CreatedAtRoute("GetSchedule", new { controller = "Schedules", id = schedule.Id }, schedule);
            return result;
        }
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]ScheduleViewModel schedule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Schedule _scheduleDb = _scheduleRepository.GetSingle(id);
            if (_scheduleDb == null)
            {
                return NotFound();
            } else
            {
                _scheduleDb.Title = schedule.Title;
                _scheduleDb.Location = schedule.Location;
                _scheduleDb.Description = schedule.Desceiption;
                _scheduleDb.Status = (ScheduleStatus)Enum.Parse(typeof(ScheduleStatus), schedule.Status);
                _scheduleDb.Type = (ScheduleType)Enum.Parse(typeof(ScheduleType), schedule.Type);
                _scheduleDb.TimeStart = schedule.TimeStart;
                _scheduleDb.TimeEnd = schedule.TimeEnd;
                _attendeeRepository.DeleteWhere(a => a.ScheduleId == id);
                foreach (var item in schedule.Attedndess)
                {
                    _scheduleDb.Attendees.Add(new Attendee { ScheduleId = id, UserId = item });
                }
                _scheduleRepository.Commit();
            }
            schedule = Mapper.Map<Schedule, ScheduleViewModel>(_scheduleDb);
            return new NoContentResult();
        }
        [HttpDelete("{id}", Name ="RemoveSchedule")]
        public IActionResult Delete(int id)
        {
            Schedule _scheduleDb = _scheduleRepository.GetSingle(id);
            if (_scheduleDb == null)
            {
                return new NotFoundResult();
            }
            _attendeeRepository.DeleteWhere(a => a.ScheduleId == id );
            _scheduleRepository.Delete(_scheduleDb);
            _scheduleRepository.Commit();
            return new NoContentResult();

        }
        [HttpDelete("{id}/removeattendee/{attendee}")]
        public IActionResult Delete(int id, int attendee)
        {
            Schedule _scheduleDb = _scheduleRepository.GetSingle(id);
            if (_scheduleDb == null)
            {
                return new NotFoundResult();
            }
            _attendeeRepository.DeleteWhere(a => a.ScheduleId == id && a.UserId == attendee);
            _attendeeRepository.Commit();
            return new NoContentResult();
        }
      
    }
}
