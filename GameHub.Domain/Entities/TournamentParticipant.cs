using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Domain.Entities
{
    public class TournamentParticipant
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string Username { get; set; }
        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }

    }
}
