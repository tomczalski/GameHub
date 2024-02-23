using AutoMapper;
using GameHub.Application.Tournament;
using GameHub.Application.Tournament.Commands.AddParticipant;
using GameHub.Application.Tournament.Commands.EditTournament;
using GameHub.Application.Tournament.Commands.Tournament;
using GameHub.Application.Tournament.Queries.GetAllGames;
using GameHub.Application.Tournament.Queries.GetAllTournamentParticipants;
using GameHub.Application.Tournament.Queries.GetTournamentByEncodedName;
using GameHub.MVC.Extensions;
using GameHub.MVC.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.InteropServices;

namespace GameHub.MVC.Controllers
{
    public class TournamentController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public TournamentController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var gamesOption = await _mediator.Send(new GetAllGamesQuery());
            ViewBag.GamesSelectList = gamesOption.Select(game => new SelectListItem
            {
                Value = game.Id.ToString(),
                Text = game.GameName
            }).ToList();

            // ViewBag.SelectGameOptions = selectGameOptions;

            return View();
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateTournamentCommand command)
        {
            ModelState.Remove("Game.Tournaments");
            if (!ModelState.IsValid)
            {
                var gamesOption = await _mediator.Send(new GetAllGamesQuery());
                ViewBag.GamesSelectList = gamesOption.Select(game => new SelectListItem
                {
                    Value = game.Id.ToString(),
                    Text = game.GameName
                }).ToList();
                return View(command);
            }
            await _mediator.Send(command);
            this.SetNotification("success", $"Utworzono turniej: {command.Name}");
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> AddParticipant(AddParticipantCommand command, string encodedName)
        {
            ModelState.Remove("UserId");
            if (!ModelState.IsValid)
            {

                var tournamentDto = await _mediator.Send(new GetTournamentByEncodedNameQuery(encodedName));
                var participantsDto = await _mediator.Send(new GetAllTournamentParticipantsQuery(encodedName));

                var model = new TournamentDetailsViewModel
                {
                    Tournament = tournamentDto,
                    Participants = participantsDto
                };


                return View("Details", model);
            }

            command.EncodedName = encodedName;
            await _mediator.Send(command);

            return RedirectToAction("Details", new { encodedName = encodedName });
        }
        [Route("Tournament/{encodedName}/Details")]
        public async Task<IActionResult> Details(string encodedName)
        {
            var tournamentDto = await _mediator.Send(new GetTournamentByEncodedNameQuery(encodedName));
            var participantsDto = await _mediator.Send(new GetAllTournamentParticipantsQuery(encodedName));

            var model = new TournamentDetailsViewModel
            {
                Tournament = tournamentDto,
                Participants = participantsDto
            };

            return View(model);
        }
        
        [Route("Tournament/{encodedName}/Edit")]
        public async Task<IActionResult> Edit(string encodedName)
        {
            var dto = await _mediator.Send(new GetTournamentByEncodedNameQuery(encodedName));

            if (!dto.IsEditable)
            {
                return RedirectToAction("NoAccess", "Home");
            }

            EditTournamentCommand model = _mapper.Map<EditTournamentCommand>(dto);
            return View(model);
        }
        [HttpPost]
        [Route("Tournament/{encodedName}/Edit")]
        public async Task<IActionResult> Edit(string encodedName, EditTournamentCommand command)
        {
            if (!ModelState.IsValid)
            {
                return View(command);
            }
            await _mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }
    }
}
