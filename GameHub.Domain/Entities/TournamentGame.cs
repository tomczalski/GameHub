using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Domain.Entities
{
    public class TournamentGame
    {
        public int Id { get; set; }
        public string? GameName { get; set; }
        public int MaxPlayers { get; set; }
        public ICollection<Tournament> Tournaments { get; set; }
        
    }
}
