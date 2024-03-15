using AutoMapper;
using GameHub.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.Tournament.Queries.GetTournamentByEncodedName
{
    internal class GetTournamentByEncodedNameQueryHandler : IRequestHandler<GetTournamentByEncodedNameQuery, TournamentDto>
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IMapper _mapper;

        public GetTournamentByEncodedNameQueryHandler(ITournamentRepository tournamentRepository, IMapper mapper)
        {
            _tournamentRepository = tournamentRepository;
            _mapper = mapper;
        }

        public async Task<TournamentDto> Handle(GetTournamentByEncodedNameQuery request, CancellationToken cancellationToken)
        {
            var tournament = await _tournamentRepository.GetByEncodedName(request.EncodedName);
            var dto = _mapper.Map<TournamentDto>(tournament);
            return dto;
        }
    }
}
