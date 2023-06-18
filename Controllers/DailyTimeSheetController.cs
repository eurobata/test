using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TemplateManagementSystem.BLL;
using TemplateManagementSystem.DAL;
using TemplateManagementSystem.Models;

namespace TemplateManagementSystem.Controllers
{
    [Authorize]
    public class DailyTimeSheetController : Controller
    {
        // GET: DailyTimeSheet
        public ActionResult Generate(int? id ,string prev)
        {
            ViewBag.pre=prev;
            var manger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var CurrentUser = manger.FindById(User.Identity.GetUserId());
            ViewBag.ProfilePicture = CurrentUser.ProfilePicture;

            DailyTimeSheetViewModel model = new DailyTimeSheetViewModel();
            if (id.HasValue)
            {
                model = new DailyTimeSheetLogic().FetchById(id.Value);
            }
            else
            {
                model.lstDetails.Add(new DailyTimeSheetDetailViewModel());
                model.lstDetails.Add(new DailyTimeSheetDetailViewModel());
                model.lstDetails.Add(new DailyTimeSheetDetailViewModel());
            }
            return View(model);
       
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(DailyTimeSheetViewModel model)
        {
            string userId = User.Identity.GetUserId();
            DailyTimeSheet timeSheet = new DailyTimeSheet();
            List<TimeSheetDetail> lstDetails = new List<TimeSheetDetail>();
            timeSheet.Name = model.Name;
            timeSheet.Location = model.Location;
            timeSheet.CreatedDate = DateTime.Now;
            timeSheet.CreatedBy = userId;
            timeSheet.Department = model.Department;
            foreach (var item in model.lstDetails)
            {
                lstDetails.Add(new TimeSheetDetail()
                {
                    Initials = item.Initials,
                    JobDescription = item.JobDescription,
                    TimeStarted = item.TimeStarted,
                    TimeStopped = item.TimeStopped
                });
            }
            if (model.Id > 0)
            {
                timeSheet.Id = model.Id;
                timeSheet.ModifiedDate = DateTime.Now;
                timeSheet.ModifiedBy = userId;
            }
            timeSheet.TimeSheetDetails = lstDetails;
            ViewAsPdf pdf = new Rotativa.PartialViewAsPdf("_DailyTimeSheet", model)
            {
                FileName = string.Format("{0}.pdf", Guid.NewGuid()),
                CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
            };
            timeSheet.FileName = pdf.FileName;
            string message = new DailyTimeSheetLogic().SaveUpdate(timeSheet,model.type);



            byte[] pdfData = pdf.BuildFile(ControllerContext);
            string fullPath = Path.Combine(Server.MapPath("~/Reports"), pdf.FileName);
            using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            {
                fileStream.Write(pdfData, 0, pdfData.Length);
            }

            //var user = new Common().GetById(userId);
            //string body = "Hello " + String.Format("{0} {1}", user.FirstName, user.LastName) + ", <br/><br />";
            //body += model.EmailBody;
            //if (new Common().SendEmail(model.Email, model.Subject, body, fullPath, model.CC))
            //{

            //}

            return Redirect("~/TemplateHistory/Created");

        }

        public ActionResult Download(int? id)
        {
            string fileName = new DailyTimeSheetLogic().FetchFile(id.Value);
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Reports/" + fileName));
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}