using Scheduler.API.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Scheduler.API.ViewModels
{
    public class ScheduleViewModel : IValidatableObject
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Desceiption { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public DateTime DataCreated { get; set; }
        public DateTime DataUpdate { get; set; }
        public string Creator { get; set; }
        public int CreatorId { get; set; }
        public int[] Attedndess { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new ScheduleViewModelValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}
