using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BOSAPI.Entities;

namespace BOSAPI.Models
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }


        public AuthenticateResponse(User user, string token)
        {
            Id = user.UserId;
            Username = user.UserName;
            Token = token;
        }
    }
}
