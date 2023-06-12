﻿using AutoMapper;
using GameHub.Application.ApplicationUser;
using GameHub.Application.Tournament.Commands.Tournament;
using GameHub.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.Tournament.Commands.CreateTournament
{
    public class CreateTournamentCommandHandler : IRequestHandler<CreateTournamentCommand>
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public CreateTournamentCommandHandler(ITournamentRepository tournamentRepository, IMapper mapper, IUserContext userContext)
        {
            _tournamentRepository = tournamentRepository;
            _mapper = mapper;
            _userContext = userContext;
        }
        public async Task<Unit> Handle(CreateTournamentCommand request, CancellationToken cancellationToken)
        {
            var tournament = _mapper.Map<Domain.Entities.Tournament>(request);
            tournament.EncodeName();

            tournament.CreatedById = _userContext.GetCurrentUser().Id;

            await _tournamentRepository.Create(tournament);

            return Unit.Value;
        }
    }
}