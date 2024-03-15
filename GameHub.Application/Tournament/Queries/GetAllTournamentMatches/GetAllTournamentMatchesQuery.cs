using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.Tournament.Queries.GetAllTournamentMatches
{
    public class GetAllTournamentMatchesQuery : IRequest<List<MatchDto>>
    {
        public string EncodedName { get; set; }

        public GetAllTournamentMatchesQuery(string encodedName)
        {
            EncodedName = encodedName;
        }
    }
}
