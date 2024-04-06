using GameHub.Application.Tournament;
using GameHub.Application.Tournament.Queries.GetAllGames;
using GameHub.Application.Tournament.Queries.GetAllTournamentParticipants;
using GameHub.Application.Tournament.Queries.GetAllTournaments;
using GameHub.Application.Tournament.Queries.GetAllTournamentTeams;
using GameHub.Application.Tournament.Queries.GetTournamentByEncodedName;
using GameHub.Application.Tournament.Queries.GetTournamentByGame;
using GameHub.MVC.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Drawing;

namespace GameHub.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMediator _mediator;

        public HomeController(ILogger<HomeController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<IActionResult> Index(int? SelectedGameId)
        {
            List<TournamentDto> tournamentDtos;

            if (SelectedGameId.HasValue)
            {
                tournamentDtos = await _mediator.Send(new GetTournamentByGameQuery(SelectedGameId.Value));
            }
            else
            {
                tournamentDtos = await _mediator.Send(new GetAllTournamentsQuery());
            }
            var games = await _mediator.Send(new GetAllGamesQuery());
            var model = new IndexModel
            {   
                Tournaments = tournamentDtos,
                SelectedGameId = SelectedGameId,
                Games = games,
            };

            return View(model);
        }

        public async Task<IActionResult> ArchiveTournaments()
        {
            var tournaments = await _mediator.Send(new GetAllTournamentsQuery());
            return View(tournaments);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}