using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Domain.Entities
{
    public class TeamMember
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }

    }
}
