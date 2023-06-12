﻿using AutoMapper;
using GameHub.Application.Tournament.Commands.EditTournament;
using GameHub.Application.Tournament.Commands.Tournament;
using GameHub.Application.Tournament.Queries.GetTournamentByEncodedName;
using GameHub.MVC.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Create()
        {
     
            return View();
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateTournamentCommand command) 
        {
            if (!ModelState.IsValid)
            {
                return View(command);
            }
            await _mediator.Send(command);
            this.SetNotification("success", $"Utworzono turniej: {command.Name}");
            return RedirectToAction("Index", "Home");
        }
        
        [Route("Tournament/{encodedName}/Details")]
        public async Task<IActionResult> Details(string encodedName)
        {
            var dto = await _mediator.Send(new GetTournamentByEncodedNameQuery(encodedName));
            return View(dto);
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