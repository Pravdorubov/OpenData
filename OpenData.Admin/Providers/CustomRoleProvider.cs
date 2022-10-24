using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Helpers;
using System.Security.Cryptography;
using System.Web.WebPages;
using Microsoft.Internal.Web.Utils;
using OpenData.Admin.Models;
using OpenData.Domain.Abstract;
using OpenData.Domain.Entities;
using OpenData.Domain.Concrete;
using Ninject;

namespace OpenData.Admin.Providers
{
    public class CustomRoleProvider : RoleProvider
    {
        [Inject]
        public IURepository repository { get; set; }  
        
        public override string[] GetRolesForUser(string login)
        {
            string[] role = new string[] { };
                try
                {
                    // Получаем пользователя


                    User user = repository.Users.Where(u => u.Login == login).FirstOrDefault();
                    if (user != null)
                    {
                        // получаем роль

                        Role userRole = repository.GetRole(user.RoleID);

                        if (userRole != null)
                        {
                            role = new string[] { userRole.Name };
                        }
                    }
                }
                catch
                {
                    role = new string[] { };
                //}
            }
            return role;
        }

        public override void CreateRole(string roleName)
        {
            Role newRole = new Role() { Name = roleName };
            repository.SaveRole(newRole);
        }
        
        public override bool IsUserInRole(string username, string roleName)
        {
            bool outputResult = false;
            // Находим пользователя
                try
                {
                    // Получаем пользователя
                    User user = repository.Users.Where(u => u.Login == username).FirstOrDefault();
                    if (user != null)
                    {
                        // получаем роль

                        Role userRole = repository.GetRole(user.RoleID);

                        //сравниваем
                        if (userRole != null && userRole.Name == roleName)
                        {
                            outputResult = true;
                        }
                    }
                }
                catch
                {
                    outputResult = false;
                }
            //}
            return outputResult;
        }
        
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}