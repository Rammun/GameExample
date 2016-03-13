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
            var battle = new UserBattlesViewModel
            {
                UserParam1 = new UserParam { Id = string.Empty, Name = User.Identity.Name },
                UserParam2 = new UserParam { Id = string.Empty, Name = string.Empty },
                SelectUser = new UserParam { Id = string.Empty, Name = string.Empty },
                Users = new List<UserParam>()
            };

            return View(battle);
        }
    }
}