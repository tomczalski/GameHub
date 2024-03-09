using GameHub.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.Tournament
{
    public class TeamMemberDto
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public int TeamId { get; set; }
    }
}
