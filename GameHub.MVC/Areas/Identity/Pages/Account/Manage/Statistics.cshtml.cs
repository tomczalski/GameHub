using GameHub.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using GameHub.Infrastructure.Repositories;
using GameHub.Domain.Interfaces;

namespace GameHub.MVC.Areas.Identity.Pages.Account.Manage
{
    public class StatisticsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<ChangePasswordModel> _logger;
        private readonly ITournamentRepository _tournamentRepository;

        public StatisticsModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<ChangePasswordModel> logger,
            ITournamentRepository tournamentRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _tournamentRepository = tournamentRepository;
        }

        
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userTournaments = await _tournamentRepository.GetAllUserTournaments(user.Id);
            var wonTournaments = await _tournamentRepository.GetWonTournamentsForUserAsync(user.Id);
            int totalTournaments = userTournaments.Count();
            int totalWonTournaments = wonTournaments.Count();

            ViewData["UserTournaments"] = userTournaments;
            ViewData["TotalTournaments"] = totalTournaments;
            ViewData["WonTournaments"] = wonTournaments;
            ViewData["TotalWonTournaments"] = totalWonTournaments;

            return Page();
        }
    }
}

