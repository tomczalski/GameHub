using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Domain.Entities
{
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

        public string? CreatedById { get; set; }
        public IdentityUser? CreatedBy { get; set; }
        public ICollection<TournamentParticipant>? Participants { get; set; }

        public void EncodeName() => EncodedName = Name.ToLower().Replace(" ", "-");
    }
}
