using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Domain.Entities
{
    public class Round
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RoundNumber { get; set; }
        public Tournament Tournament { get; set; }
        public int MaxRounds { get; private set; }

        public Round() { }  
        public Round(Tournament tournament)
        {
            Tournament = tournament;
            MaxRounds = Tournament.MaxRounds;
        }
    }
}
