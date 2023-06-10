using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Domain.Entities
{
    public class Tournament
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Game { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public string? Prize { get; set; }
        public string EncodedName { get; private set; } = default!;
        public int NumberOfTeams { get; set; }

        public void EncodeName() => EncodedName = Name.ToLower().Replace(" ", "-");
    }
}
