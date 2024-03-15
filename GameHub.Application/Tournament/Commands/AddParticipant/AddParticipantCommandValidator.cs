using FluentValidation;
using GameHub.Application.ApplicationUser;
using GameHub.Application.Tournament.Commands.Tournament;
using GameHub.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.Tournament.Commands.AddParticipant
{
    public class AddParticipantCommandValidator : AbstractValidator<AddParticipantCommand>
    {
        public AddParticipantCommandValidator(ITournamentRepository _repository)
        {

            RuleFor(c => c.TournamentId).NotEmpty().WithMessage("Pole nie może byc puste!");

            RuleFor(x => x)
                .Custom((command, context) =>
                {
                    var alreadySignedUp = _repository.IsUserAlreadyRegistered(command.TournamentId);

                    if (alreadySignedUp)
                    {
                        context.AddFailure("");
                    }
                });


        }
    }
}
