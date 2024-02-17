using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.Tournament
{
    public class TournamentGameDto
    {
        public int Id { get; set; }
        public string? GameName { get; set; }
        public int MaxPlayers { get; set; }
    }
}
