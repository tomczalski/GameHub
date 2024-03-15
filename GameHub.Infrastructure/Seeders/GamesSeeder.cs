using GameHub.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Infrastructure.Seeders
{
    public class GamesSeeder
    {
        private readonly GameHubDbContext _dbContext;
        public GamesSeeder(GameHubDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task Seed() 
        { 
            if(await _dbContext.Database.CanConnectAsync()) 
            {
               // if(_dbContext.TournamentGame.Any)
            }
        }
    }
}
