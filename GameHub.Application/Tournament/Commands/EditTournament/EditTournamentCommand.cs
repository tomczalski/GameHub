﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.Tournament.Commands.EditTournament
{
    public class EditTournamentCommand : TournamentDto, IRequest
    {
    }
}
