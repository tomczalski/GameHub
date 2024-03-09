using GameHub.Application.ApplicationUser;
using GameHub.Application.Interface;
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

        private readonly IGameHubDbContext _dbContext;
        private readonly IUserContext _userContext;
        public TournamentRepository(IGameHubDbContext dbContext, IUserContext userContext)
        {
            _userContext = userContext;
            _dbContext = dbContext;
        }

        public async Task<int> Create(Tournament tournament)
        {
            var tournamentId = _dbContext.Tournaments.Add(tournament);
            await _dbContext.SaveChangesAsync();
            return tournamentId.Entity.Id;
        }

        public async Task Edit()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddParticipant(TournamentParticipant tournamentParticipant)
        {
            _dbContext.TournamentParticipants.Add(tournamentParticipant);
            await _dbContext.SaveChangesAsync();
        }
        public async Task AddTeam(Team team)
        {
            _dbContext.Teams.Add(team);
            await _dbContext.SaveChangesAsync();
        }

        public async Task JoinTeam(TeamMember teamMember)
        {
            _dbContext.TeamMembers.Add(teamMember);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<Tournament>> GetAll() => await _dbContext.Tournaments.Include(x => x.Game).ToListAsync();
        public async Task<IEnumerable<TournamentGame>> GetAllGames() => await _dbContext.TournamentGames.ToListAsync();
        public async Task<Tournament> GetByEncodedName(string encodedName) => await _dbContext.Tournaments.Include(x => x.Game).FirstAsync(c => c.EncodedName == encodedName);
        public async Task<IEnumerable<TournamentParticipant>> GetAllParticipants() => await _dbContext.TournamentParticipants.ToArrayAsync();
        public async Task<IEnumerable<TeamMember>> GetAllTeamMembers() => await _dbContext.TeamMembers.ToArrayAsync();
        public async Task<IEnumerable<Team>> GetAllTeams() => await _dbContext.Teams.Include(x => x.TeamMembers).ToArrayAsync();
        public bool IsUserAlreadyRegistered(int tournamentId)
        {
            var userId = _userContext.GetCurrentUser().Id;
            var existingParticipant = _dbContext.TournamentParticipants.FirstOrDefault(p => p.UserId == userId && p.TournamentId == tournamentId);

            return existingParticipant != null;
        }
        public bool IsUserAlreadyInTeam(int tournamentId, int teamId)
        {
            var userId = _userContext.GetCurrentUser().Id;
            var teams = _dbContext.Teams.Where(x => x.TournamentId == tournamentId).Include(x => x.TeamMembers).ToList();
            var isExist = false;
            foreach ( var team in teams )
            {
                foreach (var teamMember in team.TeamMembers)
                {
                    if (teamMember.UserId == userId)
                    {
                        isExist = true;
                        return isExist;
                    }
                }
            }
           // var existingParticipant = _dbContext.TeamMembers.Include(tm => tm.Team).FirstOrDefault(p => p.UserId == userId);

            return isExist;
        }

        public async Task<TournamentGame> GetGameById(int gameId)
        {
            return await _dbContext.TournamentGames.FirstOrDefaultAsync(c => c.Id == gameId);
        }
    }
}
