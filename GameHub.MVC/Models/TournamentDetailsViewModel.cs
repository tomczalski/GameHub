using GameHub.Application.Tournament;
using GameHub.Domain.Entities;

namespace GameHub.MVC.Models
{
    public class TournamentDetailsViewModel
    {
        public TournamentDto Tournament { get; set; }
        public List<TournamentParticipantDto> Participants { get; set; }
        public List<TeamDto> TournamentTeams { get; set; }
    }
}
