using GameHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.Tournament
{
    public class MatchDto
    {
        public int Id { get; set; }
        public int RoundId { get; set; }
        public Round Round { get; set; }
        public int Team1Id { get; set; }
        public Team Team1 { get; set; }
        public int Team2Id { get; set; }
        public Team Team2 { get; set; }
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }
        public int WinnerId { get; set; }
        public MatchState MatchState { get; set; }

    }
}
