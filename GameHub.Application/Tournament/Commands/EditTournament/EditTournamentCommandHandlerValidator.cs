using FluentValidation;
using FluentValidation.Validators;
using GameHub.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.Tournament.Commands.EditTournament
{
    public class EditTournamentCommandHandlerValidator : AbstractValidator<EditTournamentCommand>
    {
        public EditTournamentCommandHandlerValidator(ITournamentRepository repository)
        {
            RuleFor(c => c.Prize).NotEmpty().WithMessage("Pole nie może byc puste!");
            RuleFor(c => c.NumberOfTeams).NotEmpty().WithMessage("Pole nie może byc puste!");
            RuleFor(c => c.StartDate).NotEmpty().WithMessage("Pole nie może byc puste!");
            RuleFor(c => c.Game).NotEmpty().WithMessage("Pole nie może byc puste!");
        }
    }
}
