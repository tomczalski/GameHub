using AutoMapper;
using GameHub.Application.Tournament.Queries.GetAllTournaments;
using GameHub.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.Tournament.Queries.GetAllTournamentTeams
{
    internal class GetAllTournamentTeamsQueryHandler : IRequestHandler<GetAllTournamentTeamsQuery, List<TeamDto>>
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IMapper _mapper;

        public GetAllTournamentTeamsQueryHandler(ITournamentRepository tournamentRepository, IMapper mapper)
        {
            _tournamentRepository = tournamentRepository;
            _mapper = mapper;
        }
        public async Task<List<TeamDto>> Handle(GetAllTournamentTeamsQuery request, CancellationToken cancellationToken)
        {
            var teams = await _tournamentRepository.GetAllTeams();
            var tournament = await _tournamentRepository.GetByEncodedName(request.EncodedName);

            var teamsList = new List<TeamDto>();


            foreach (var item in teams)
            {
                if (item.TournamentId == tournament.Id)
                {
                    teamsList.Add(new TeamDto()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        TeamSize = item.TeamSize,
                        TournamentId = item.TournamentId
                    });
                }
            }
            return teamsList;
        }
    }
}
