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
        Task Create(Tournament tournament);
        Task<IEnumerable<Tournament>> GetAll();
        Task<IEnumerable<TournamentGame>> GetAllGames();
        Task<IEnumerable<TournamentParticipant>> GetAllParticipants();
        Task<Tournament>GetByEncodedName(string encodedName);
        Task Edit();
        Task AddParticipant(TournamentParticipant tournamentParticipant);
        public bool IsUserAlreadyRegistered(string userId, int tournamentId);

    }
}
