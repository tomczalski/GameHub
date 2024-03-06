using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.Tournament
{
    public class TournamentParticipantDto
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public int TournamentId { get; set; }

    }

}
