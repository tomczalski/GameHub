using GameHub.Domain.Entities;
using GameHub.Domain.Interfaces;
using GameHub.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<Tournament>> GetAll() => await _dbContext.Tournaments.ToListAsync();

        public async Task<Tournament> GetByEncodedName(string encodedName) =>await _dbContext.Tournaments.FirstAsync(c => c.EncodedName == encodedName);


        
    }
}
