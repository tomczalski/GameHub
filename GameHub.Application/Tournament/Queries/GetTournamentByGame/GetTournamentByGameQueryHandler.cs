using AutoMapper;
using GameHub.Application.Tournament.Queries.GetAllTournaments;
using GameHub.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.Tournament.Queries.GetTournamentByGame
{
    public class GetTournamentByGameQueryHandler : IRequestHandler<GetTournamentByGameQuery, List<TournamentDto>>
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IMapper _mapper;

        public GetTournamentByGameQueryHandler(ITournamentRepository tournamentRepository, IMapper mapper)
        {
            _tournamentRepository = tournamentRepository;
            _mapper = mapper;
        }
        public async Task<List<TournamentDto>> Handle(GetTournamentByGameQuery request, CancellationToken cancellationToken)
        {
            var tournaments = await _tournamentRepository.GetByGame(request.GameId);
            var dtos = _mapper.Map<List<TournamentDto>>(tournaments);

            return dtos;
        }
    }
}
