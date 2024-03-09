﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.Tournament.Commands.JoinTeam
{
    public class JoinTeamCommand : TeamMemberDto, IRequest
    {
        public string EncodedName { get; set; }
        public int TournamentTeamId { get; set; }
        public int TournamentId { get; set; }
    }
}
