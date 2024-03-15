using AutoMapper;
using GameHub.Application.Tournament.Queries.GetAllTournaments;
using GameHub.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GameHub.Application.Tournament.Queries.GetAllGames
{
    public class GetAllGamesQueryHandler : IRequestHandler<GetAllGamesQuery, List<TournamentGameDto>>
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IMapper _mapper;

        public GetAllGamesQueryHandler(ITournamentRepository tournamentRepository, IMapper mapper)
        {
            _tournamentRepository = tournamentRepository;
            _mapper = mapper;
        }
        public async Task<List<TournamentGameDto>> Handle(GetAllGamesQuery request, CancellationToken cancellationToken)
        {


            var tournaments = await _tournamentRepository.GetAllGames();

            var tournamet = new List<TournamentGameDto>();
            foreach (var item in tournaments)
            {
                tournamet.Add(new TournamentGameDto()
                {
                    Id = item.Id,
                    GameName = item.GameName,
                    MaxPlayers = item.MaxPlayers
                });
            }
            //var dtos = _mapper.Map<IEnumerable<TournamentGameDto>>(tournaments);

            return tournamet;
        }


    }
}
