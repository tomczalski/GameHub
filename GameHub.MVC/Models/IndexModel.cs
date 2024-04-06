using GameHub.Application.Tournament;

namespace GameHub.MVC.Models
{
    public class IndexModel
    {
        public TournamentDto Tournament { get; set; }
        public List<TournamentDto> Tournaments { get; set; }
        public List<TournamentParticipantDto> Participants { get; set; }
        public int? SelectedGameId { get; set; }
        public List<TournamentGameDto> Games { get; set; }
    }
}
