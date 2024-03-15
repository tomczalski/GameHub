using AutoMapper;
using GameHub.Application.Tournament.Queries.GetAllGames;
using GameHub.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.Tournament.Queries.GetAllTournamentParticipants
{
    public class GetAllTournamentParticipantsQueryHandler : IRequestHandler<GetAllTournamentParticipantsQuery, List<TournamentParticipantDto>>
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IMapper _mapper;

        public GetAllTournamentParticipantsQueryHandler(ITournamentRepository tournamentRepository, IMapper mapper)
        {
            _tournamentRepository = tournamentRepository;
            _mapper = mapper;
        }
        public async Task<List<TournamentParticipantDto>> Handle(GetAllTournamentParticipantsQuery request, CancellationToken cancellationToken)
        {
            var tournamentParticipants = await _tournamentRepository.GetAllParticipants();
            var tournament = await _tournamentRepository.GetByEncodedName(request.EncodedName);

            var participants = new List<TournamentParticipantDto>();


                foreach (var item in tournamentParticipants)
                {
                if (item.TournamentId == tournament.Id)
                {
                    participants.Add(new TournamentParticipantDto()
                    {
                        UserId = item.UserId,
                        Username = item.Username,
                        TournamentId = item.TournamentId
                    });
                }
                }
            return participants;
        }
    }
}
