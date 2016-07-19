﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheduler.API.ViewModels.Validations
{
    public class ScheduleViewModelValidator : AbstractValidator<ScheduleViewModel>
    {
        public ScheduleViewModelValidator()
        {
            RuleFor(s => s.TimeEnd).Must((start, end) =>
             {
                 return DateTimeIsGreater(start.TimeStart, end);
             }).WithMessage("Schedule's End Timie must be grater than Start Time");

        }
        private bool DateTimeIsGreater(DateTime start,DateTime end)
        {
            return end > start;
        }
    }
}
