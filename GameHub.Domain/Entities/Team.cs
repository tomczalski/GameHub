using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Domain.Entities
{
    public class Team
    {
        private static int _teamNumber = 1;

        public int Id { get; set; }
        public string Name { get; set; }
        public int TeamSize { get; set; } 
        public int TournamentId { get; set; }
        // public ICollection<Match> Matches { get; set; }
        public Tournament Tournament { get; set; }
        public ICollection<TeamMember>? TeamMembers { get; set; }

        public Team()
        {
            Name = "Team " + _teamNumber++;
        }


        public Team(int maxPlayers)
        {
            Name = "Team " + _teamNumber++;
            TeamSize = maxPlayers;
        }
    }
}
