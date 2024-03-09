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
        Task<Tournament>GetByEncodedName(string encodedName);
        Task Edit();
        Task AddParticipant(TournamentParticipant tournamentParticipant);
        Task JoinTeam(TeamMember teamMember);
        public bool IsUserAlreadyRegistered(int tournamentId);
        public bool IsUserAlreadyInTeam(int tournamentId, int teamId);
        Task<TournamentGame> GetGameById(int gameId);
        Task<IEnumerable<Team>> GetAllTeams();

    }
}
