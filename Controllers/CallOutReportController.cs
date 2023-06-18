using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Rotativa;
using System;
using System.Collections.Generic;
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
    public class CallOutReportController : Controller
    {
        // GET: CallOutReport
        public ActionResult Generate(int? id,string prev)
        {

            ViewBag.pre = prev;
            var manger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var CurrentUser = manger.FindById(User.Identity.GetUserId());
            ViewBag.ProfilePicture = CurrentUser.ProfilePicture;

            CallOutReportViewModel model = new CallOutReportViewModel();
            if (id.HasValue)
            {
                model = new CallOutReportLogic().FetchById(id.Value);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(CallOutReportViewModel model)
        {
            string userId = User.Identity.GetUserId();
            CallOutReport callOutReport = new CallOutReport();
            callOutReport.ClientName = model.ClientName;
            callOutReport.JobAddress = model.JobAddress;
            callOutReport.ContactTelephone = model.ContactTelephone;
            callOutReport.CreatedBy = userId;
            callOutReport.CreatedDate = DateTime.Now;
            callOutReport.EngineerName = model.EngineerName;
            callOutReport.Date = Convert.ToDateTime(model.DateStr) + DateTime.Now.TimeOfDay;
            model.Date = Convert.ToDateTime(model.DateStr) + DateTime.Now.TimeOfDay;
            callOutReport.DetailOfWork = model.DetailOfWork;
            callOutReport.ArrivalTime = model.ArrivalTime;
            callOutReport.DepartTime = model.DepartTime;
            callOutReport.JobNo = model.JobNo;

            if (model.Id > 0)
            {
                callOutReport.Id = model.Id;
                callOutReport.ModifiedDate = DateTime.Now;
                callOutReport.ModifiedBy = userId;
            }

            ViewAsPdf pdf = new Rotativa.PartialViewAsPdf("_CallOutReport", model)
            {
                FileName = string.Format("{0}.pdf", Guid.NewGuid()),
                CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
            };
            callOutReport.FileName = pdf.FileName;
            string message = new CallOutReportLogic().SaveUpdate(callOutReport,model.type);

            
            byte[] pdfData = pdf.BuildFile(ControllerContext);
            string fullPath = Path.Combine(Server.MapPath("~/Reports"), pdf.FileName);
            using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            {
                fileStream.Write(pdfData, 0, pdfData.Length);
            }
            //var user = new Common().GetById(userId);
            //string body = "Hello " + String.Format("{0} {1}", user.FirstName, user.LastName) + ", <br/><br />";
            //body += model.EmailBody;
            //if (new Common().SendEmail(model.Email, model.Subject, body, fullPath,model.Cc))
            //{

            //}
            return Redirect("~/templatehistory/Created");

        }

        public ActionResult Download(int? id)
        {
            string fileName = new CallOutReportLogic().FetchFile(id.Value);
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Reports/" + fileName));
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}