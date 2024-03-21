using GameHub.Application.Interface;
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
    public class GameHubDbContext : IdentityDbContext<ApplicationUser>, IGameHubDbContext
    {
        public GameHubDbContext(DbContextOptions<GameHubDbContext> options) : base(options) 
        {

        }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<TournamentGame> TournamentGames { get; set; }
        public DbSet<TournamentParticipant> TournamentParticipants { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Round> Rounds { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            builder.Entity<Match>().HasOne<Team>(m => m.Team1).WithMany().HasForeignKey(m => m.Team1Id).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Match>().HasOne<Team>(m => m.Team2).WithMany().HasForeignKey(m => m.Team2Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
