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
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public int GameId { get; set; }
        public TournamentGame Game { get; set; } = new TournamentGame();
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public int Prize { get; set; }
        public string? EncodedName { get; set; }
        public int NumberOfTeams { get; set; }
        public bool IsEditable { get; set; }
        public TournamentState TournamentState { get; set; }
        public int MaxRounds { get; private set; }
        public int WinnerId { get; set; }
        public int MaxPlayers { get; private set; }
        public ICollection<TournamentParticipant>? Participants { get; set; }
    }
}
