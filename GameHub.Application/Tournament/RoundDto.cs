using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.Tournament
{
    public class RoundDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RoundNumber { get; set; }
        public int MaxRounds { get; private set; }
    }
}
