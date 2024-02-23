﻿using GameHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.Interface
{
    public interface IGameHubDbContext
    {
        public DbSet<GameHub.Domain.Entities.Tournament> Tournaments { get; set; }
        public DbSet<TournamentGame> TournamentGames { get; set; }
        public DbSet<TournamentParticipant> TournamentParticipants { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}