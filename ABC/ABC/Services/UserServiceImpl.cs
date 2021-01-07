using ABC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABC.Services
{
    public class UserServiceImpl
    {
        private ThakshilawaEntities2 db = new ThakshilawaEntities2();

        public List<DataDTO> GetRolesForUser(int userid)
        {
            try
            {
                var role = (from dd in db.AccessLevelUsers
                            join pp in db.AccessLevels on dd.accesslevelid equals pp.id
                            where dd.userid ==userid
                            
                            select new DataDTO
                            {
                                 id =pp.id,
                                 name = pp.access
                             }).ToList();

                return role;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        public List<UserData> GetUsersForSearch(string username)
        {
            try
            {
               // appUser userData = db.appUsers.FirstOrDefault(u => u.username == username);
                List<UserData> userData = db.UserDatas.Where(u => u.userid.Equals(username)).ToList();

                return userData;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
    }
}