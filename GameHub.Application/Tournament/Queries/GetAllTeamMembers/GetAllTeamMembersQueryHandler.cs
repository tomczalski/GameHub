using AutoMapper;
using GameHub.Application.Tournament.Queries.GetAllTournamentParticipants;
using GameHub.Domain.Entities;
using GameHub.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.Tournament.Queries.GetAllTeamMembers
{
    public class GetAllTeamMembersQueryHandler : IRequestHandler<GetAllTeamMembersQuery, List<TeamMemberDto>>
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IMapper _mapper;

        public GetAllTeamMembersQueryHandler(ITournamentRepository tournamentRepository, IMapper mapper)
        {
            _tournamentRepository = tournamentRepository;
            _mapper = mapper;
        }
        public async Task<List<TeamMemberDto>> Handle(GetAllTeamMembersQuery request, CancellationToken cancellationToken)
        {
            var teamMembers = await _tournamentRepository.GetAllTeamMembers();
            var members = new List<TeamMemberDto>();
            int teamId = request.TournamentTeamId;

            foreach (var item in teamMembers)
            {
                if (item.TeamId == teamId)
                {
                    members.Add(new TeamMemberDto()
                    {
                        UserId = item.UserId,
                        Username = item.Username,
                        TeamId = item.TeamId,
                    });
                }
            }
            return members;
        }
    }
}
