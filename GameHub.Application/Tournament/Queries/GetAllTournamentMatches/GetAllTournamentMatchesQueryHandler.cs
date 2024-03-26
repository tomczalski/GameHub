using AutoMapper;
using GameHub.Application.Tournament.Queries.GetAllTournamentTeams;
using GameHub.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.Tournament.Queries.GetAllTournamentMatches
{
    public class GetAllTournamentMatchesQueryHandler : IRequestHandler<GetAllTournamentMatchesQuery, List<MatchDto>>
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IMapper _mapper;

        public GetAllTournamentMatchesQueryHandler(ITournamentRepository tournamentRepository, IMapper mapper)
        {
            _tournamentRepository = tournamentRepository;
            _mapper = mapper;
        }
        public async Task<List<MatchDto>> Handle(GetAllTournamentMatchesQuery request, CancellationToken cancellationToken)
        {
            var teams = await _tournamentRepository.GetAllTeams();
            var tournament = await _tournamentRepository.GetByEncodedName(request.EncodedName);
            var matches = await _tournamentRepository.GetAllMatches();

            var matchesList = new List<MatchDto>();

            foreach (var item in matches) 
            {
                if (item.Round.Tournament.Id == tournament.Id)
                {
                    var match = new MatchDto()
                    {
                        Id = item.Id,
                        RoundId = item.RoundId,
                        Round = item.Round,
                        Team1Id = item.Team1Id,
                        Team1 = item.Team1,
                        Team2Id = item.Team2Id,
                        Team2 = item.Team2,
                        Team1Score = item.Team1Score,
                        Team2Score = item.Team2Score,
                        WinnerId = item.WinnerId,
                        MatchState = item.MatchState,
                    };

                    matchesList.Add(match);
                }
            }
            return matchesList;
        }
    }
}
