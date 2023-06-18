using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TemplateManagementSystem.BLL;
using TemplateManagementSystem.DAL;
using TemplateManagementSystem.Models;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Configuration;

namespace TemplateManagementSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller

    {

        TemplateManagementSystemEntities db = new TemplateManagementSystemEntities();
        public ActionResult panel()
        {
            if (ConfigurationManager.AppSettings["adminId"] != User.Identity.GetUserId())
            {
                DAL.SubscriptionModel subscription = new SubcriptionLogic().subscription(User.Identity.GetUserId());
                //if (subscription == null || subscription.AppUserId == null)
                //{

                //    ApplicationDbContext _Context = new ApplicationDbContext();
                //    var user = _Context.Users.Find(User.Identity.GetUserId());
                //    _Context.Users.Remove(user);
                //    _Context.SaveChanges();
                //    var AuthenticationManager = HttpContext.GetOwinContext().Authentication;
                //    AuthenticationManager.SignOut();
                //    return Content("Subscription Fails Please Try Again...");


                //}


            }

            var manger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var CurrentUser = manger.FindById(User.Identity.GetUserId());
            ViewBag.ProfileName = CurrentUser.Email;
            ViewBag.FirstName = CurrentUser.FirstName;
            ViewBag.LastName = CurrentUser.LastName;
            ViewBag.Email = CurrentUser.Email;
            ViewBag.Address = CurrentUser.Address;
            ViewBag.ProfilePicture = CurrentUser.ProfilePicture;
            ViewBag.Phone = CurrentUser.PhoneNumber;
            ViewBag.Favourties = JsonConvert.SerializeObject(new FavouriteLogic().FetchFavourties(User.Identity.GetUserId()));

            var templates = new TemplateHistoryLogic().GetTemplates();
            var templateCategories = new CategoryLogic().GetTemplateCategories();
            TemplateSearchModel templateModel;
            List<TemplateSearchModel> lstModel = new List<TemplateSearchModel>();
            foreach (var item in templates)
            {
                templateModel = new TemplateSearchModel();
                templateModel.TemplateName = item.Text;
                templateModel.TemplateClass = item.Text.Replace(" ", "");
                int templateId = Convert.ToInt32(item.Value);
                templateModel.TemplateCategory = string.Join(",", templateCategories.Where(s => s.TemplateId == templateId).Select(s => s.Category.CategoryName).ToList());
                lstModel.Add(templateModel);
            }

            ViewBag.lstTemplates = lstModel;
            return View();
        }

        [HttpPost]
        public void AddToFavourite(string templateType, string add)
        {
            string userId = User.Identity.GetUserId();
            new FavouriteLogic().AddToFavourite(userId, templateType, add);
        }

       public ActionResult termsConditions()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}