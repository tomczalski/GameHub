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
        Task<Tournament>GetByEncodedName(string encodedName);
    }
}
