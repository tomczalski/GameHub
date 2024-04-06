using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.ApplicationUser
{
    public class CurrentUser
    {
        public CurrentUser(string id, string email, string nickname, int balance, IEnumerable<string> roles)
        {
            Id = id;
            Email = email;
            Nickname = nickname;
            Balance = balance;
            Roles = roles;

        }
        public string Id { get; set; }
        public string Email { get; set; }
        public string Nickname { get; }
        public int Balance { get; set; }
        public IEnumerable<string> Roles { get; set; }

        public bool IsInRole(string role) => Roles.Contains(role);
    }
}
