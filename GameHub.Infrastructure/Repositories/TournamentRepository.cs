using GameHub.Application.ApplicationUser;
using GameHub.Application.Interface;
using GameHub.Domain.Entities;
using GameHub.Domain.Interfaces;
using GameHub.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections;
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

        public async Task UpdateTournamentState(Tournament tournament)
        {
            var teams = await _dbContext.Teams.Where(x => x.TournamentId == tournament.Id).Include(x => x.TeamMembers).ToListAsync();
            bool allTeamsFull = true;

            foreach (var team in teams)
            {
                if (team.TeamMembers.Count() != team.TeamSize)
                {
                    allTeamsFull = false;
                    break;
                }
            }

            if (allTeamsFull)
            {
                tournament.TournamentState = TournamentState.Started;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task GenerateScheudle(int tournamentId)
        {
            var tournament = await _dbContext.Tournaments
                .Include(t => t.Teams)
                .FirstOrDefaultAsync(t => t.Id == tournamentId);

            bool isTournamentStarted = false;
            if (tournament.TournamentState == TournamentState.Started)
            {
                isTournamentStarted = true;
            }

            if (isTournamentStarted)
            {
                var rounds = new List<Round>();
                for (int roundNumber = 1; roundNumber <= tournament.MaxRounds; roundNumber++)
                {
                    var round = new Round
                    {
                        Name = "Round " + roundNumber,
                        RoundNumber = roundNumber,
                        Tournament = tournament,
                        MaxRounds = tournament.MaxRounds
                    };
                    rounds.Add(round);
                    _dbContext.Rounds.Add(round);
                }

                var teams = tournament.Teams.ToList();
                var shuffledTeams = teams.OrderBy(t => Guid.NewGuid()).ToList();

                for (int i = 0; i < shuffledTeams.Count; i += 2)
                {
                    var match = new Match
                    {
                        Round = rounds[0],
                        Team1 = shuffledTeams[i],
                        Team2 = shuffledTeams[i + 1]
                    };
                    _dbContext.Matches.Add(match);
                }
                await _dbContext.SaveChangesAsync();
            }  
        }

        public async Task AdvanceTeams(int tournamentId, int roundId)
        {
            var tournament = await _dbContext.Tournaments.FirstOrDefaultAsync(t => t.Id == tournamentId);
            var rounds = await _dbContext.Rounds.Where(t => t.Tournament == tournament).ToListAsync();
            var currentRound = await _dbContext.Rounds.Where(r => r.Id == roundId).FirstOrDefaultAsync();
            var matches = await _dbContext.Matches.Where(m => m.RoundId == roundId).ToListAsync();
            var lastRound = rounds.Last();

            if (tournament == null)
            {
                throw new ArgumentException("Nie znaleziono turnieju.");
            }

            var nextRoundNumber = currentRound.RoundNumber + 1;
            var nextRound = rounds.FirstOrDefault(r => r.RoundNumber == nextRoundNumber);

            if (nextRound == null)
            {
                tournament.TournamentState = TournamentState.Ended;
                var winner = await _dbContext.Matches.Where(m => m.RoundId == lastRound.Id).FirstOrDefaultAsync();
                tournament.WinnerId = winner.WinnerId;
                var winners = await _dbContext.TeamMembers.Where(tm => tm.TeamId == winner.WinnerId).ToListAsync();

                foreach (var winningTeamMember in winners)
                {
                    var user = await _dbContext.Users.Where(u => u.Id == winningTeamMember.UserId).FirstOrDefaultAsync();
                    user.Balance += tournament.Prize;
                }
                await _dbContext.SaveChangesAsync();
                return;
            }

            if (matches.All(m => m.WinnerId != 0))
            {
                var advancedTeamIds = matches.Where(m => m.WinnerId != 0).Select(m => m.WinnerId).ToList();
                var advancedTeams = await _dbContext.Teams.Where(t => advancedTeamIds.Contains(t.Id)).ToListAsync();

                if (advancedTeams.Count % 2 != 0)
                {
                    var advancedTeam = advancedTeams[advancedTeams.Count - 1];
                    advancedTeams.RemoveAt(advancedTeams.Count - 1);

                    var advancedMatch = new Match
                    {
                        Round = nextRound,
                        Team1 = advancedTeam,
                        Team2 = advancedTeam,
                        Team1Score = 3,
                        Team2Score = 0,
                        WinnerId = advancedTeam.Id
                    };
                    _dbContext.Matches.Add(advancedMatch);
                }

                for (int i = 0; i < advancedTeams.Count; i+=2)
                {
                    var match = new Match
                    {
                        Round = nextRound,
                        Team1 = advancedTeams[i],
                        Team2 = advancedTeams[i + 1]
                    };
                    _dbContext.Matches.Add(match);
                }
                await _dbContext.SaveChangesAsync();
            }  
        }
        public async Task<IEnumerable<Tournament>> GetAllUserTournaments(string userId)
        {
            var tournamentIds = await _dbContext.TournamentParticipants.Where(u => u.UserId == userId).Select(tp => tp.TournamentId).ToListAsync();
            var userTournaments = await _dbContext.Tournaments.Where(t => tournamentIds.Contains(t.Id)).ToListAsync();

            return userTournaments;

        }
        public async Task<List<Tournament>> GetWonTournamentsForUserAsync(string userId)
        {
            var tournamentIds = await _dbContext.TournamentParticipants.Where(u => u.UserId == userId).Select(tp => tp.TournamentId).ToListAsync();
            var userTournaments = await _dbContext.Tournaments.Where(t => tournamentIds.Contains(t.Id)).ToListAsync();
            var teamIds = await _dbContext.TeamMembers
                                   .Where(tm => tm.UserId == userId)
                                   .Select(tm => tm.TeamId)
                                   .ToListAsync();
            var userTeams = await _dbContext.Teams.Where(team => teamIds.Contains(team.Id)).ToListAsync();
            var wonTournamentIds = _dbContext.Tournaments
         .Where(t => t.WinnerId != null).AsEnumerable().Where(t => userTeams.Any(ut => ut.Id == t.WinnerId)).Select(t => t.Id).ToList();
            var wonTournaments = await _dbContext.Tournaments.Where(t => wonTournamentIds.Contains(t.Id)).ToListAsync();

            return wonTournaments;
        }
        public async Task<IEnumerable<Tournament>> GetAll() => await _dbContext.Tournaments.Include(x => x.Game).ToListAsync();
        public async Task<IEnumerable<TournamentGame>> GetAllGames() => await _dbContext.TournamentGames.ToListAsync();
        public async Task<Tournament> GetByEncodedName(string encodedName) => await _dbContext.Tournaments.Include(x => x.Game).FirstAsync(c => c.EncodedName == encodedName);
        public async Task<IEnumerable<TournamentParticipant>> GetAllParticipants() => await _dbContext.TournamentParticipants.ToArrayAsync();
        public async Task<IEnumerable<TeamMember>> GetAllTeamMembers() => await _dbContext.TeamMembers.ToArrayAsync();
        public async Task<IEnumerable<Team>> GetAllTeams() => await _dbContext.Teams.Include(x => x.TeamMembers).ToArrayAsync();
        public async Task<IEnumerable<Match>> GetAllMatches() => await _dbContext.Matches.Include(x => x.Round).ThenInclude(t => t.Tournament).ToArrayAsync();
        public async Task<Match> GetMatchById(int matchId) => await _dbContext.Matches.Include(x => x.Round).ThenInclude(t => t.Tournament).FirstAsync(m => m.Id == matchId);
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


        public int GetTeamSize(int tournamentTeamId)
        {
            var teamDetails = _dbContext.Teams.FirstOrDefault(team => team.Id == tournamentTeamId);

            if (teamDetails != null)
            {
                return teamDetails.TeamSize;
            }
            else
            { 
                throw new Exception("Team size not found for the given team id.");
            }
        }

        public int GetNumberOfUsersInTeam(int tournamentTeamId)
        {
            var numberOfUsers = _dbContext.TeamMembers.Count(member => member.TeamId == tournamentTeamId);
            return numberOfUsers;
        }


    }
}
