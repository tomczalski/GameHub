using AutoMapper;
using GameHub.Application.ApplicationUser;
using GameHub.Application.Tournament.Commands.Tournament;
using GameHub.Domain.Entities;
using GameHub.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.Tournament.Commands.AddParticipant
{
    public class AddParticipantCommandHandler : IRequestHandler<AddParticipantCommand>
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IUserContext _userContext;
        private readonly IMapper _mapper;

        public AddParticipantCommandHandler(ITournamentRepository tournamentRepository, IUserContext userContext, IMapper mapper)
        {
            _tournamentRepository = tournamentRepository;
            _userContext = userContext;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(AddParticipantCommand request, CancellationToken cancellationToken)
        {
            var user = _userContext.GetCurrentUser();
            var tournament = await _tournamentRepository.GetByEncodedName(request.EncodedName);
            var dto = _mapper.Map<TournamentDto>(tournament);
            var tournamentId = dto.Id;

            var participant = MapToTornamentParticipant(request);

            participant.UserId = user.Id;
            participant.TournamentId = tournamentId;

            await _tournamentRepository.AddParticipant(participant);
            return Unit.Value;
        }

        private Domain.Entities.TournamentParticipant MapToTornamentParticipant(AddParticipantCommand request)
        {
            var participant = new Domain.Entities.TournamentParticipant()
            {
                UserId = request.UserId,
                TournamentId = request.TournamentId,
            };
            return participant;
        }



    }
}
