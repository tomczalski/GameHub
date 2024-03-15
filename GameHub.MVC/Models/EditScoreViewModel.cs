using GameHub.Application.Tournament;
using GameHub.Application.Tournament.Commands.EditScore;

namespace GameHub.MVC.Models
{
    public class EditScoreViewModel
    {
        public TournamentDto Tournament { get; set; }
        public List<MatchDto> Matches { get; set; }
        public MatchDto MatchForm { get; set; }
    }
}
