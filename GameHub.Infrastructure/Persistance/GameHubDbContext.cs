using GameHub.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Infrastructure.Persistance
{
    public class GameHubDbContext : IdentityDbContext
    {
        public GameHubDbContext(DbContextOptions<GameHubDbContext> options) : base(options) 
        {

        }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<TournamentGame> TournamentGames { get; set; }
        public DbSet<TournamentParticipant> TournamentParticipants { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }
    }
}
