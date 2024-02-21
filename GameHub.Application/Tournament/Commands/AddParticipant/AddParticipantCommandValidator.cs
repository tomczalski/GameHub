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
        public AddParticipantCommandValidator(ITournamentRepository _repository, UserContext userContext)
        {
            RuleFor(c => c.UserId).NotEmpty().WithMessage("Pole nie może byc puste!");
            RuleFor(c => c.TournamentId).NotEmpty().WithMessage("Pole nie może byc puste!");

           
            RuleFor(c => new { c.UserId, c.TournamentId }).Must((command, ids) => _repository.IsUserAlreadyRegistered(userContext.GetCurrentUser().Id, ids.TournamentId))
           .WithMessage("Użytkownik jest już zapisany do tego turnieju.");

        }
    }
}
