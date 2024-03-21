using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.ApplicationUser
{
    public class CurrentUser
    {
        public CurrentUser(string id, string email, string nickname, int balance)
        {
            Id = id;
            Email = email;
            Nickname = nickname;
            Balance = balance;
        }
        public string Id { get; set; }
        public string Email { get; set; }
        public string Nickname { get; }
        public int Balance { get; set; }
    }
}
