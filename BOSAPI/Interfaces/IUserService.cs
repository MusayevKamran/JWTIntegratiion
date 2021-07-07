using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BOSAPI.Entities;
using BOSAPI.Models;

namespace BOSAPI.Interfaces
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<User> GetAll();
        User GetById(int id);
    }
}
