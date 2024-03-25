using FluentValidation;
using GameHub.Application.ApplicationUser;
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
        public CreateTournamentCommandValidator(ITournamentRepository repository, IUserContext userContext)
        {
            RuleFor(c => c.Name).NotEmpty().MinimumLength(8).MaximumLength(32).WithMessage("Pole nie może byc puste!");

            RuleFor(c => c.Game).NotEmpty().WithMessage("Pole nie może byc puste!");

            RuleFor(c => c.NumberOfTeams)
            .GreaterThanOrEqualTo(2).WithMessage("Liczba drużyn musi wynosić więcej niż 2.")
            .Must(BeEven).WithMessage("Liczba drużyn musi być liczbą parzystą.");

            RuleFor(c => c.StartDate)
                .GreaterThan(DateTime.Now).WithMessage("Turniej musi odbywać się w przyszłości.");

            RuleFor(c => c)
            .Must(command =>
            {
                int userBalance = userContext.GetCurrentUser().Balance;
                return userBalance >= command.Prize;
            })
      .WithMessage("Saldo użytkownika jest za niskie, aby utworzyć turniej z taką nagrodą.");
        }
        private bool BeEven(int number)
        {
            return number % 2 == 0;
        }
    }
}
