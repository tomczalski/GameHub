using GameHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Domain.Interfaces
{
    public interface ITournamentRepository
    {
        Task<int> Create(Tournament tournament);
        Task AddTeam(Team team);
        Task<IEnumerable<Tournament>> GetAll();
        Task<IEnumerable<TournamentGame>> GetAllGames();
        Task<IEnumerable<TournamentParticipant>> GetAllParticipants();
        Task<IEnumerable<TeamMember>> GetAllTeamMembers();
        Task<IEnumerable<Match>> GetAllMatches();
        Task<Tournament>GetByEncodedName(string encodedName);
        Task<Match> GetMatchById(int matchId);
        Task Edit();
        Task AddParticipant(TournamentParticipant tournamentParticipant);
        Task JoinTeam(TeamMember teamMember);
        Task UpdateTournamentState(Tournament tournament);
        Task GenerateScheudle(int tournamentId);
        Task AdvanceTeams(int tournamentId, int roundId);
        public bool IsUserAlreadyRegistered(int tournamentId);
        public bool IsUserAlreadyInTeam(int tournamentId, int teamId);
        public int GetTeamSize(int tournamentTeamId);
        public int GetNumberOfUsersInTeam(int tournamentTeamId);
        Task<TournamentGame> GetGameById(int gameId);
        Task<IEnumerable<Team>> GetAllTeams();

    }
}
