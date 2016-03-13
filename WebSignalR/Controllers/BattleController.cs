using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSignalR.Models;

namespace WebSignalR.Controllers
{
    [Authorize]
    public class BattleController : Controller
    {
        // GET: Battle
        public ActionResult Index()
        {
            return View(new UserParam { Id = string.Empty, Name = User.Identity.Name });
        }
    }
}