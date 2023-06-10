using GameHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.Tournament
{
    public class TournamentDto
    {
        public string Name { get; set; } = default!;
        public string Game { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public string? Prize { get; set; }
        public string? EncodedName { get; set; }
        public int NumberOfTeams { get; set; }
    }
}
