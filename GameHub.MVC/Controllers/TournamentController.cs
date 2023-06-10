using GameHub.Application.Tournament.Commands.Tournament;
using GameHub.Application.Tournament.Queries.GetTournamentByEncodedName;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace GameHub.MVC.Controllers
{
    public class TournamentController : Controller
    {
        private readonly IMediator _mediator;

        public TournamentController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateTournamentCommand command) 
        {
            if (!ModelState.IsValid)
            {
                return View(command);
            }
            await _mediator.Send(command);
            return RedirectToAction("Index", "Home");
        }

        [Route("Tournament/{encodedName}/Details")]
        public async Task<IActionResult> Details(string encodedName)
        {
            var dto = await _mediator.Send(new GetTournamentByEncodedNameQuery(encodedName));
            return View(dto);
        }
    }
}
