using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.Tournament.Queries.GetAllTournamentParticipants
{
    public class GetAllTournamentParticipantsQuery : IRequest<List<TournamentParticipantDto>>
    {
        public string EncodedName { get; set; }

        public GetAllTournamentParticipantsQuery(string encodedName)
        {
            EncodedName = encodedName;
        }
    }
}
