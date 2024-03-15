using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.Tournament
{
    public class TeamDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TeamSize { get; set; }
        public int TournamentId { get; set; }
        public List<string> Members { get; set; }
    }
}
