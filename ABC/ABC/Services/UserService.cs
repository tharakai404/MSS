using ABC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABC.Services
{
    public interface UserService
    {
        List<DataDTO> GetRolesForUser(int userid);
        List<UserData> GetUsersForSearch(string username);
    }
}