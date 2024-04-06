namespace GameHub.MVC.Models.Bracket
{
    public class BracketRoundMatchViewModel
    {
        public string Team1Name { get; set; }
        public string Team2Name { get; set; }
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }
        public bool IsVisible { get; set; }
    }
}
