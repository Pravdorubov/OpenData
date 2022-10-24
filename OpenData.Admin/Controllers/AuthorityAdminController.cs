using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpenData.Admin.Controllers
{
    [Authorize (Roles="AuthorityAdmin")]
    public class AuthorityAdminController : Controller
    {
        //
        // GET: /AuthorityAdmin/

        public ActionResult Index()
        {
            return View();
        }

    }
}
