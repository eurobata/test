using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TemplateManagementSystem.BLL;

namespace TemplateManagementSystem.Controllers
{
    public class FavouritesController : Controller
    {
        // GET: Favourites
        public ActionResult List()
        {
            var fav = new FavouriteLogic().FetchFavourties(User.Identity.GetUserId());
            return View(fav);
        }
    }
}