using GameHub.Application.Tournament;

namespace GameHub.MVC.Models.Bracket
{
    public class BracketViewModel
    {
        public List<BracketRoundViewModel> Rounds { get; set; }
        public TournamentDto Tournament { get; set; }
    }
}
