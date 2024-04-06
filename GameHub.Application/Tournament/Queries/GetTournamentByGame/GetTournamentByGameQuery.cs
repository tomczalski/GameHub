using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.Tournament.Queries.GetTournamentByGame
{
    public class GetTournamentByGameQuery : IRequest<List<TournamentDto>>
    {
        public int GameId { get; set; }

        public GetTournamentByGameQuery(int gameId)
        {
            GameId = gameId;
        }
    }
}
