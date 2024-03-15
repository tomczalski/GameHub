using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.Tournament.Queries.GetAllTeamMembers
{
    public class GetAllTeamMembersQuery : IRequest<List<TeamMemberDto>>
    {
        public int TournamentTeamId { get; set; }

        public GetAllTeamMembersQuery(int tournamentTeamId)
        {
            TournamentTeamId = tournamentTeamId;
        }
    }
}
