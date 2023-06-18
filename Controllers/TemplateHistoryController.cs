using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TemplateManagementSystem.BLL;
using TemplateManagementSystem.Models;

namespace TemplateManagementSystem.Controllers
{
    [Authorize]
    public class TemplateHistoryController : Controller
    {
        ApplicationDbContext ApplicationDbContext = new ApplicationDbContext();
        // GET: TemplateHistory
        public ActionResult Created()
        {
            var manger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var CurrentUser = manger.FindById(User.Identity.GetUserId());
            ViewBag.ProfilePicture = CurrentUser.ProfilePicture;

            string currentUser = User.Identity.GetUserId();

            string endDate = string.Format("{0:MM/dd/yyyy 23:59:59}", DateTime.Now);
            var model = new TemplateHistoryLogic().FetchHistory(DateTime.Now.AddMonths(-1).Date, currentUser, DateTime.ParseExact(endDate, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture));

            var CurrentUserName = ApplicationDbContext.Users.Find(currentUser);
            string name = CurrentUserName.FirstName + " " + CurrentUserName.LastName;
            model = model.Where(x => x.CreatedBy == name).ToList();


            return View(model);
        }

        public ActionResult FetchEmail(int id, string type)
        {
            EmailSetup model = new EmailSetup();
            model.id = id;
            model.templete = type;
            return PartialView("_Email",model);
        }

        [HttpPost]
        public ActionResult email(EmailSetup model)
        {
            if (model.templete == "DailyTimeSheet")
            {
                string fullPath = Path.Combine(Server.MapPath("~/Reports"), new DailyTimeSheetLogic().FetchFile(model.id));

                if (!string.IsNullOrEmpty(fullPath))
                {
                    string userId = User.Identity.GetUserId();

                    var user = new Common().GetById(userId);
                    string body = "Hello " + String.Format("{0} {1}", user.FirstName, user.LastName) + ", <br/><br />";
                    body += model.EmailBody;
                    if (new Common().SendEmail(model.Email, model.Subject, body, fullPath, model.CC))
                    {

                    }

                }
            }
            else if (model.templete == "Customer Templete")
            {
                string fullPath = Path.Combine(Server.MapPath("~/Reports"), new CustomerTempleteLogic().FetchFile(model.id));

                if (!string.IsNullOrEmpty(fullPath))
                {
                    string userId = User.Identity.GetUserId();

                    var user = new Common().GetById(userId);
                    string body = "Hello " + String.Format("{0} {1}", user.FirstName, user.LastName) + ", <br/><br />";
                    body += model.EmailBody;
                    if (new Common().SendEmail(model.Email, model.Subject, body, fullPath, model.CC))
                    {

                    }

                }
            }
            else if (model.templete == "Bussiness Quotes")
            {
                string fullPath = Path.Combine(Server.MapPath("~/Reports"), new BussinessQoutesLogic().FetchFile(model.id));

                if (!string.IsNullOrEmpty(fullPath))
                {
                    string userId = User.Identity.GetUserId();

                    var user = new Common().GetById(userId);
                    string body = "Hello " + String.Format("{0} {1}", user.FirstName, user.LastName) + ", <br/><br />";
                    body += model.EmailBody;
                    if (new Common().SendEmail(model.Email, model.Subject, body, fullPath, model.CC))
                    {

                    }

                }
            }
            else if (model.templete == "Pressure Washing Estimate Templete")
            {
                string fullPath = Path.Combine(Server.MapPath("~/Reports"), new WashingTempleteLogic().FetchFile(model.id));

                if (!string.IsNullOrEmpty(fullPath))
                {
                    string userId = User.Identity.GetUserId();

                    var user = new Common().GetById(userId);
                    string body = "Hello " + String.Format("{0} {1}", user.FirstName, user.LastName) + ", <br/><br />";
                    body += model.EmailBody;
                    if (new Common().SendEmail(model.Email, model.Subject, body, fullPath, model.CC))
                    {

                    }

                }
            }
            else if (model.templete == "Template5")
            {
                string fullPath = Path.Combine(Server.MapPath("~/Reports"), new Template5Logic().FetchFile(model.id));

                if (!string.IsNullOrEmpty(fullPath))
                {
                    string userId = User.Identity.GetUserId();

                    var user = new Common().GetById(userId);
                    string body = "Hello " + String.Format("{0} {1}", user.FirstName, user.LastName) + ", <br/><br />";
                    body += model.EmailBody;
                    if (new Common().SendEmail(model.Email, model.Subject, body, fullPath, model.CC))
                    {

                    }

                }
            }
            else if (model.templete == "CallOutReport")
            {
                string fullPath = Path.Combine(Server.MapPath("~/Reports"), new Template5Logic().FetchFile(model.id));

                if (!string.IsNullOrEmpty(fullPath))
                {
                    string userId = User.Identity.GetUserId();

                    var user = new Common().GetById(userId);
                    string body = "Hello " + String.Format("{0} {1}", user.FirstName, user.LastName) + ", <br/><br />";
                    body += model.EmailBody;
                    if (new Common().SendEmail(model.Email, model.Subject, body, fullPath, model.CC))
                    {

                    }

                }
            }

            return RedirectToAction("Created");

        }

        public string FetchFile(int id, string type)
        {
            return new DailyTimeSheetLogic().FetchFile(id, type);
            //return Server.MapPath("~/Reports/" + fileName);
            //return Server.MapPath("~/Reports/" + fileName);
            //byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Reports/" + fileName));
        }
    }
}