using GameHub.Application.Tournament.Queries.GetAllTournaments;
using GameHub.MVC.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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

        public async Task<IActionResult> Index()
        {
            var tournaments = await _mediator.Send(new GetAllTournamentsQuery());
            return View(tournaments);
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