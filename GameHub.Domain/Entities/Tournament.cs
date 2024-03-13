using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Domain.Entities
{
    public enum TournamentState
    {
        Awaiting,
        Started,
        Ended
    }
    public class Tournament
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public int GameId { get; set; }
        public TournamentGame Game { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public string? Prize { get; set; }
        public string EncodedName { get; private set; } = default!;
        public int NumberOfTeams { get; set; }
        public int MaxRounds { get; private set; } = default!;


        public string? CreatedById { get; set; }
        public IdentityUser? CreatedBy { get; set; }
        public ICollection<TournamentParticipant>? Participants { get; set; }
        public ICollection<Team>? Teams { get; set; }
        public TournamentState TournamentState { get; set; } = TournamentState.Awaiting;

        public void EncodeName() => EncodedName = Name.ToLower().Replace(" ", "-");
        public void CalculateMaxRounds() 
        {
            double logBase2 = Math.Log(NumberOfTeams, 2);
            int rounds = (int)Math.Ceiling(logBase2);
            MaxRounds = rounds;
        }
    }
}
