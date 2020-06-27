using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    [Authorize]
    public class RedirectController : Controller
    {
        // GET: Redirect
        public ActionResult Index()
        {
            return View();
        }
    }
}