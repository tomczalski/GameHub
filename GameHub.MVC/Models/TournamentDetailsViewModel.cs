using GameHub.Application.Tournament;

namespace GameHub.MVC.Models
{
    public class TournamentDetailsViewModel
    {
        public TournamentDto Tournament { get; set; }
        public List<TournamentParticipantDto> Participants { get; set; }
    }
}
