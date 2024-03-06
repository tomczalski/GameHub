using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.Tournament.Queries.GetAllTournamentTeams
{
    public class GetAllTournamentTeamsQuery : IRequest<List<TeamDto>>
    {
        public string EncodedName { get; set; }

        public GetAllTournamentTeamsQuery(string encodedName)
        {
            EncodedName = encodedName;
        }
    }
}
