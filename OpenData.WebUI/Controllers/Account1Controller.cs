using System.Web.Mvc;
using OpenData.WebUI.Infrastructure.Abstract;
using OpenData.WebUI.Models;

namespace OpenData.WebUI.Controllers
{
    public class Account1Controller : Controller
    {
        IAuthProvider authProvider;
        public Account1Controller(IAuthProvider auth)
        {
            authProvider = auth;
        }
        
        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (authProvider.Authenticate(model.UserName, model.Password))
                {
                    return Redirect(returnUrl ?? Url.Action("Index", "Admin"));
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect username or password");
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

    }
}
