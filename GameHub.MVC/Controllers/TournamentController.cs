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
using GameHub.MVC.Models.Bracket;
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
        private BracketViewModel MapMatchesToBracketViewModel(List<MatchDto> matchDtos, int maxRounds)
        {
            var barcketViewModel = new BracketViewModel() { Rounds = new List<BracketRoundViewModel>() };
            var groupedMatch = matchDtos.OrderBy(x => x.RoundId).GroupBy(x => x.RoundId).ToList();

            int maxMatchInRounds = (int)Math.Pow(2, maxRounds) / 2;
            foreach (var round in groupedMatch)
            {

                BracketRoundViewModel bracketRoundViewModel = new BracketRoundViewModel() { BracketRoundMatches = new List<BracketRoundMatchViewModel>() };
                foreach (var matchPair in round)
                {
                    var matchPairModel = new BracketRoundMatchViewModel()
                    {
                        Team1Name = matchPair.Team1.Name,
                        Team1Score = matchPair.Team1Score,
                        Team2Name = matchPair.Team2.Name,
                        Team2Score = matchPair.Team2Score,
                        IsVisible = true
                    };
                    bracketRoundViewModel.BracketRoundMatches.Add(matchPairModel);
                }
                while (bracketRoundViewModel.BracketRoundMatches.Count < maxMatchInRounds)
                {
                    var matchPairModel = new BracketRoundMatchViewModel()
                    {
                        Team1Name = "",
                        Team1Score = 0,
                        Team2Name = "",
                        Team2Score = 0,
                        IsVisible = false
                    };
                    bracketRoundViewModel.BracketRoundMatches.Add(matchPairModel);
                }
                barcketViewModel.Rounds.Add(bracketRoundViewModel);
                maxMatchInRounds = maxMatchInRounds / 2;
            }


            if (maxRounds >= groupedMatch.Count)
            {
                for (int i = 1; i <= maxRounds; i++)
                {
                    BracketRoundViewModel bracketRoundViewModel = new BracketRoundViewModel() { BracketRoundMatches = new List<BracketRoundMatchViewModel>() };
                    if (groupedMatch.Count < i)
                    {
                        int liczbaMeczy = (int)Math.Ceiling((double)maxRounds / i);
                        for (int j = 0; j < liczbaMeczy; j++)
                        {
                            var matchPairModel = new BracketRoundMatchViewModel()
                            {
                                Team1Name = "",
                                Team1Score = 0,
                                Team2Name = "",
                                Team2Score = 0,
                                IsVisible = true
                            };
                            bracketRoundViewModel.BracketRoundMatches.Add(matchPairModel);
                        }
                        barcketViewModel.Rounds.Add(bracketRoundViewModel);
                    }
                }
            }

            return barcketViewModel;
        }
        [Route("Tournament/{encodedName}/Bracket")]
        public async Task<IActionResult> Bracket(string encodedName)
        {
            var matchesDto = await _mediator.Send(new GetAllTournamentMatchesQuery(encodedName));
            var tournamentDto = await _mediator.Send(new GetTournamentByEncodedNameQuery(encodedName));
            var model = MapMatchesToBracketViewModel(matchesDto, tournamentDto.MaxRounds);
            model.Tournament = tournamentDto;
            return View(model);
        }

        [Route("Tournament/{encodedName}/EditScore")]
        public async Task<IActionResult> EditScore(string encodedName)
        {
            var tournamentDto = await _mediator.Send(new GetTournamentByEncodedNameQuery(encodedName));
            var matchesDto = await _mediator.Send(new GetAllTournamentMatchesQuery(encodedName));
            var teamsDto = await _mediator.Send(new GetAllTournamentTeamsQuery(encodedName));

            var model = new EditScoreViewModel
            {
                Tournament = tournamentDto,
                Matches = matchesDto,
                Teams = teamsDto,
                MatchForm = new MatchDto(),
                RoundForm = new RoundDto()
            };

            return View(model);
        }
        [HttpPost]
        [Route("Tournament/{encodedName}/EditScore")]
        public async Task<IActionResult> EditScore(EditScoreViewModel scoreModel, string encodedName, int tournamentId)
        {
            ModelState.Remove("Tournament");
            ModelState.Remove("Matches");
            ModelState.Remove("Teams");
            ModelState.Remove("RoundForm");
            ModelState.Remove("MatchForm.Round");
            ModelState.Remove("MatchForm.Team1");
            ModelState.Remove("MatchForm.Team2");
            if (!ModelState.IsValid)
            {
                var tournamentDto = await _mediator.Send(new GetTournamentByEncodedNameQuery(encodedName));
                var matchesDto = await _mediator.Send(new GetAllTournamentMatchesQuery(encodedName));
                var teamsDto = await _mediator.Send(new GetAllTournamentTeamsQuery(encodedName));

                var model = new EditScoreViewModel
                {
                    Tournament = tournamentDto,
                    Matches = matchesDto,
                    Teams = teamsDto,
                    MatchForm = new MatchDto(),
                    RoundForm = new RoundDto()
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

            return RedirectToAction("EditScore", new { encodedName = encodedName });
        }

    }
}
