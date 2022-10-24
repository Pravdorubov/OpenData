using OpenData.Admin.Infrastructure.Abstract;
using OpenData.Admin.Models;
using OpenData.Admin.Providers;
using OpenData.Domain.Abstract;
using OpenData.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OpenData.Admin.Controllers
{
    public class AccountController : Controller
    {
        private IURepository repository;
        
        public AccountController(IURepository repo)
        {
            this.repository = repo;
        }
        
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        string[] role = ((CustomRoleProvider)Roles.Provider).GetRolesForUser(model.UserName);
                        switch (role[0])
                        { 
                            case "Administrator":
                                return RedirectToAction("", new { controller = "Admin", action = "Index" });
                                break;
                            case "Operator":
                                int Id = ((CustomMembershipProvider)Membership.Provider).GetId(model.UserName);
                                return RedirectToAction("", new { controller = "Operator", action = "Index", UserId=Id });
                                break;
                        }
                        
                    }
                    else
                    {
                        return RedirectToAction("Index", "Restricted");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный пароль или логин");
                }
            }
            return View(model);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Login", "Account");
        }

        //public ActionResult Register()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Register(CreateUserModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        MembershipUser membershipUser = ((CustomMembershipProvider)Membership.Provider).CreateUser(model.Login, model.Password, 2, model);

        //        if (membershipUser != null)
        //        {
        //            FormsAuthentication.SetAuthCookie(model.Login, false);
        //            //switch ()
        //            //{
        //            //    case 1:
        //            //        return RedirectToAction("Index", "SuperAdmin");
        //            //        break;
        //            //    case 2:
        //            //        return RedirectToAction("Index", "AuthorityAdmin");
        //            //        break;
        //            //    case 3:
        //            //        return RedirectToAction("Index", "Operator");
        //            //        break;
        //            //}
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Ошибка при регистрации");
        //        }
        //    }
        //    return View(model);
        //}
    }
}
    
