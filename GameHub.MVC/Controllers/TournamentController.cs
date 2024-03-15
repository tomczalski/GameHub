using AutoMapper;
using GameHub.Application.Tournament;
using GameHub.Application.Tournament.Commands.AddParticipant;
using GameHub.Application.Tournament.Commands.EditScore;
using GameHub.Application.Tournament.Commands.EditTournament;
using GameHub.Application.Tournament.Commands.JoinTeam;
using GameHub.Application.Tournament.Commands.Tournament;
using GameHub.Application.Tournament.Queries.GetAllGames;
using GameHub.Application.Tournament.Queries.GetAllTeamMembers;
using GameHub.Application.Tournament.Queries.GetAllTournamentMatches;
using GameHub.Application.Tournament.Queries.GetAllTournamentParticipants;
using GameHub.Application.Tournament.Queries.GetAllTournamentTeams;
using GameHub.Application.Tournament.Queries.GetTournamentByEncodedName;
using GameHub.Domain.Entities;
using GameHub.MVC.Extensions;
using GameHub.MVC.Models;
using Humanizer;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.InteropServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
                this.SetNotification("error", "Niepoprawne dane! Nie udało się utworzyć turnieju.");
                return View(command);
            }
            await _mediator.Send(command);
            this.SetNotification("success", $"Utworzono turniej: {command.Name}");
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddParticipant(AddParticipantCommand command, string encodedName, int tournamentTeamId)
        {
            ModelState.Remove("UserId");
            ModelState.Remove("Username");
            if (!ModelState.IsValid)
            {

                var tournamentDto = await _mediator.Send(new GetTournamentByEncodedNameQuery(encodedName));
                var participantsDto = await _mediator.Send(new GetAllTournamentParticipantsQuery(encodedName));
                var teamsDto = await _mediator.Send(new GetAllTournamentTeamsQuery(encodedName));

                var model = new TournamentDetailsViewModel
                {
                    Tournament = tournamentDto,
                    Participants = participantsDto,
                    TournamentTeams = teamsDto,
                };

                this.SetNotification("error", "Użytkownik jest już zapisany do tego turnieju!");
                return View("Details", model);
            }

            command.EncodedName = encodedName;
            await _mediator.Send(command);
            this.SetNotification("success", $"Dołączono do turnieju");
            return RedirectToAction("Details", new { encodedName = encodedName });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> JoinTeam(JoinTeamCommand command, string encodedName, int tournamentTeamId)
        {
            ModelState.Remove("UserId");
            ModelState.Remove("TeamId");
            ModelState.Remove("Username");
            if (!ModelState.IsValid)
            {

                var tournamentDto = await _mediator.Send(new GetTournamentByEncodedNameQuery(encodedName));
                var participantsDto = await _mediator.Send(new GetAllTournamentParticipantsQuery(encodedName));
                var teamsDto = await _mediator.Send(new GetAllTournamentTeamsQuery(encodedName));
                var model = new TournamentDetailsViewModel
                {
                    Tournament = tournamentDto,
                    Participants = participantsDto,
                    TournamentTeams = teamsDto,
                };

                this.SetNotification("error", $"Użytkownik jest już zapisany do tego turnieju!");
                return View("Details", model);
            }

            command.EncodedName = encodedName;
            command.TournamentTeamId = tournamentTeamId;
            await _mediator.Send(command);
            this.SetNotification("success", $"Dołączono do turnieju.");
            return RedirectToAction("Details", new { encodedName = encodedName });
        }

        [Route("Tournament/{encodedName}/Details")]
        public async Task<IActionResult> Details(string encodedName)
        {
            var tournamentDto = await _mediator.Send(new GetTournamentByEncodedNameQuery(encodedName));
            var participantsDto = await _mediator.Send(new GetAllTournamentParticipantsQuery(encodedName));
            var teamsDto = await _mediator.Send(new GetAllTournamentTeamsQuery(encodedName)); 

            var model = new TournamentDetailsViewModel
            {
                Tournament = tournamentDto,
                Participants = participantsDto,
                TournamentTeams = teamsDto,
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

        [Route("Tournament/{encodedName}/Bracket")]
        public async Task<IActionResult> Bracket(string encodedName)
        {
            var teamsDto = await _mediator.Send(new GetAllTournamentTeamsQuery(encodedName));
            var matchesDto = await _mediator.Send(new GetAllTournamentMatchesQuery(encodedName));
            var tournamentDto = await _mediator.Send(new GetTournamentByEncodedNameQuery(encodedName));
            var model = new EditScoreViewModel
            {
                Tournament = tournamentDto,
                Matches = matchesDto,
                MatchForm = new MatchDto()
            };

            
            
            return View(model);
        }

        [HttpPost]
        [Route("Tournament/{encodedName}/Bracket")]
        public async Task<IActionResult> Bracket(EditScoreViewModel scoreModel, string encodedName, int tournamentId)
        {
            ModelState.Remove("Tournament");
            ModelState.Remove("Matches");
            if (!ModelState.IsValid)
            {
                var tournamentDto = await _mediator.Send(new GetTournamentByEncodedNameQuery(encodedName));
                var matchesDto = await _mediator.Send(new GetAllTournamentMatchesQuery(encodedName));

                var model = new EditScoreViewModel
                {
                    Tournament = tournamentDto,
                    Matches = matchesDto,
                    MatchForm = new MatchDto()
                };

                return View(model);
            }
            var command = new EditScoreCommand()
            {
                Id = scoreModel.MatchForm.Id,
                RoundId = scoreModel.MatchForm.RoundId,
                Team1Id = scoreModel.MatchForm.Team1Id,
                Team2Id = scoreModel.MatchForm.Team2Id,
                Team1Score = scoreModel.MatchForm.Team1Score,
                Team2Score = scoreModel.MatchForm.Team2Score,
                TournamentId = tournamentId
            };

            await _mediator.Send(command);

            return RedirectToAction("Bracket", new { encodedName = encodedName });
        }

    }
}
