using GameHub.Application.ApplicationUser;
using GameHub.Application.Tournament.Commands.EditTournament;
using GameHub.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.Tournament.Commands.EditScore
{
    public class EditScoreCommandHandler : IRequestHandler<EditScoreCommand>
    {
        private readonly ITournamentRepository _tournamentRepository;
       

        public EditScoreCommandHandler(ITournamentRepository tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;

        }
        public async Task<Unit> Handle(EditScoreCommand request, CancellationToken cancellationToken)
        {
            var matchToEdit = await _tournamentRepository.GetMatchById(request.Id);

            matchToEdit.Team1Score = request.Team1Score;
            matchToEdit.Team2Score = request.Team2Score;

            if (matchToEdit.Team1Score > matchToEdit.Team2Score) 
            {
                matchToEdit.WinnerId = request.Team1Id;
            }
            else { matchToEdit.WinnerId = request.Team2Id; }

            await _tournamentRepository.Edit();

            return Unit.Value;
        }
    }
}
