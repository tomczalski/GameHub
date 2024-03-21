using AutoMapper;
using GameHub.Application.ApplicationUser;
using GameHub.Application.Tournament.Commands.AddParticipant;
using GameHub.Domain.Entities;
using GameHub.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.Tournament.Commands.JoinTeam
{
    public class JoinTeamCommandHandler : IRequestHandler<JoinTeamCommand>
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IUserContext _userContext;
        private readonly IMapper _mapper;

        public JoinTeamCommandHandler(ITournamentRepository tournamentRepository, IUserContext userContext, IMapper mapper)
        {
            _tournamentRepository = tournamentRepository;
            _userContext = userContext;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(JoinTeamCommand request, CancellationToken cancellationToken)
        {
            var user = _userContext.GetCurrentUser();
            var tournament = await _tournamentRepository.GetByEncodedName(request.EncodedName);
            var dto = _mapper.Map<TournamentDto>(tournament);
            var tournamentId = dto.Id;

            var tournamentParticipant = MapToTornamentParticipant(request);
            var teamMember = MapToTeamMember(request);

            teamMember.UserId = user.Id;
            teamMember.TeamId = request.TournamentTeamId;
            teamMember.Username = user.Nickname;

            tournamentParticipant.UserId = user.Id;
            tournamentParticipant.TournamentId = request.TournamentId;
            tournamentParticipant.Username = user.Nickname;

            await _tournamentRepository.JoinTeam(teamMember);
            await _tournamentRepository.AddParticipant(tournamentParticipant);
            await _tournamentRepository.UpdateTournamentState(tournament);
            await _tournamentRepository.GenerateScheudle(tournamentId);
            return Unit.Value;
        }

        private Domain.Entities.TeamMember MapToTeamMember(JoinTeamCommand request)
        {
            var teamMember = new Domain.Entities.TeamMember()
            {
                UserId = request.UserId,
                Username = request.Username,
                TeamId = request.TeamId 
            };
            return teamMember;
        }
        private Domain.Entities.TournamentParticipant MapToTornamentParticipant(JoinTeamCommand request)
        {
            var teamMember = new Domain.Entities.TournamentParticipant()
            {
                UserId = request.UserId,
                Username = request.Username,
                TournamentId = request.TournamentId
            };
            return teamMember;
        }
    }
}
