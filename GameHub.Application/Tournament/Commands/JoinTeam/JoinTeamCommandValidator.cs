using FluentValidation;
using GameHub.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.Tournament.Commands.JoinTeam
{
    public class JoinTeamCommandValidator : AbstractValidator<JoinTeamCommand>
    {
        public JoinTeamCommandValidator(ITournamentRepository _repository)
        {

            RuleFor(c => c.TournamentTeamId).NotEmpty().WithMessage("Pole nie może byc puste!");

            RuleFor(x => x)
                .Custom((command, context) =>
                {
                    var alreadySignedUp = _repository.IsUserAlreadyInTeam(command.TournamentId, command.TournamentTeamId);

                    if (alreadySignedUp)
                    {
                        context.AddFailure(" ");
                    }
                });

            RuleFor(x => x)
                .Custom((command, context) =>
                {
                    var teamSize = _repository.GetTeamSize(command.TournamentTeamId);
                    var numberOfUsersInTeam = _repository.GetNumberOfUsersInTeam(command.TournamentTeamId);

                    if (numberOfUsersInTeam >= teamSize)
                    {
                        context.AddFailure($"Drużyna jest pełna.");
                    }
                });



        }
    }
}
