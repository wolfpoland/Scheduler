using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheduler.API.ViewModels.Validations
{
    public class UserViewModelValidatorcs : AbstractValidator<UserViewModel>
    {
        public UserViewModelValidatorcs()
        {
            RuleFor(user => user.Name).NotEmpty().WithMessage("Imie nie moze byc puste");
            RuleFor(user => user.Profession).NotEmpty().WithMessage("Profesja nie moze byc pusta");
            RuleFor(user => user.Avatar).NotEmpty().WithMessage("Avatar tez");
        }
    }
}
