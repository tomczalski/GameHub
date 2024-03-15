using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.Tournament.Commands.AddParticipant
{
    public class AddParticipantCommand : TournamentParticipantDto, IRequest
    {
        public string EncodedName { get; set; }

    }
}
