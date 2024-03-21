using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.ApplicationUser
{
    public interface IUserContext
    {
        CurrentUser? GetCurrentUser();
        HttpContext GetHttpContext();
    }

    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public CurrentUser? GetCurrentUser()
        {
            var user = _httpContextAccessor?.HttpContext?.User;

            if (user == null)
            {
                throw new InvalidOperationException("Użytkownik nie istnieje");
            }

            if (user.Identity == null || !user.Identity.IsAuthenticated)
            {
                return null;
            }

            var id = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            var email = user.FindFirst(c => c.Type == ClaimTypes.Email)!.Value;

            var nicknameClaim = user.FindFirst("nickname");
            var balanceClaim = user.FindFirst("balance");

            var nickname = nicknameClaim.Value;
            var balanceValue = int.TryParse(balanceClaim.Value, out var balance);

            return new CurrentUser(id, email, nickname, balance);

        }
        public HttpContext GetHttpContext()
        {
            return _httpContextAccessor.HttpContext;
        }


    }
}
