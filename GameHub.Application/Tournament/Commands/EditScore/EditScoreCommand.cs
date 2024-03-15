using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.Tournament.Commands.EditScore
{
    public class EditScoreCommand : MatchDto, IRequest
    {
        public int TournamentId { get; set; }
    }
}
