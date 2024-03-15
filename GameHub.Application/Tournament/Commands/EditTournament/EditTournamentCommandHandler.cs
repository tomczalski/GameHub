using GameHub.Application.ApplicationUser;
using GameHub.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.Tournament.Commands.EditTournament
{
    public class EditTournamentCommandHandler : IRequestHandler<EditTournamentCommand>
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IUserContext _userContext;

        public EditTournamentCommandHandler(ITournamentRepository tournamentRepository, IUserContext userContext)
        {
            _tournamentRepository = tournamentRepository;
            _userContext = userContext;
        }
        public async Task<Unit> Handle(EditTournamentCommand request, CancellationToken cancellationToken)
        {
            var objectToEdit = await _tournamentRepository.GetByEncodedName(request.EncodedName!);

            var user = _userContext.GetCurrentUser();
            var isEditable = user != null && objectToEdit.CreatedById == user.Id;

            if (isEditable) { return Unit.Value; }

            objectToEdit.Description = request.Description;
            objectToEdit.Game = request.Game;
            objectToEdit.StartDate = request.StartDate;
            objectToEdit.Prize = request.Prize;
            objectToEdit.NumberOfTeams = request.NumberOfTeams;

            await _tournamentRepository.Edit();
            return Unit.Value;

        }
    }
}
