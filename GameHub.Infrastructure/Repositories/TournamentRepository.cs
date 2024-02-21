using GameHub.Domain.Entities;
using GameHub.Domain.Interfaces;
using GameHub.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Infrastructure.Repositories
{
    public class TournamentRepository : ITournamentRepository
    {

        private readonly GameHubDbContext _dbContext;
        public TournamentRepository(GameHubDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(Tournament tournament)
        {
            _dbContext.Add(tournament);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Edit()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddParticipant(TournamentParticipant tournamentParticipant) 
        {
            _dbContext.Add(tournamentParticipant);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Tournament>> GetAll() => await _dbContext.Tournaments.Include(x => x.Game).ToListAsync();
        public async Task<IEnumerable<TournamentGame>> GetAllGames() => await _dbContext.TournamentGames.ToListAsync();
        public async Task<Tournament> GetByEncodedName(string encodedName) => await _dbContext.Tournaments.Include(x => x.Game).FirstAsync(c => c.EncodedName == encodedName);
        public async Task<IEnumerable<TournamentParticipant>> GetAllParticipants() => await _dbContext.TournamentParticipants.ToArrayAsync();
        public bool IsUserAlreadyRegistered(string userId, int tournamentId)
        {
            var existingParticipant = _dbContext.TournamentParticipants.FirstOrDefault(p => p.UserId == userId && p.TournamentId == tournamentId);

            return existingParticipant != null;
        }
        
    }
}
