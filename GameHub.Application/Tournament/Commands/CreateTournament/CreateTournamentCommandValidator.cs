using FluentValidation;
using GameHub.Application.Tournament.Commands.Tournament;
using GameHub.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.Tournament.Commands.CreateTournament
{
    public class CreateTournamentCommandValidator : AbstractValidator<CreateTournamentCommand>
    {
        public CreateTournamentCommandValidator(ITournamentRepository repository)
        {
            RuleFor(c => c.Name).NotEmpty().MinimumLength(8).MaximumLength(32).WithMessage("Pole nie może byc puste!");
            RuleFor(c => c.Game).NotEmpty().WithMessage("Pole nie może byc puste!");
        }
    }
}
