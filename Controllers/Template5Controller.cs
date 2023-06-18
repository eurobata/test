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
    public class Template5Controller : Controller
    {
        // GET: Template5
        public ActionResult Generate(int? id, string prev)
        {

            ViewBag.pre = prev;
            var manger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var CurrentUser = manger.FindById(User.Identity.GetUserId());
            ViewBag.ProfilePicture = CurrentUser.ProfilePicture;

            Template5ViewModel model = new Template5ViewModel();
            if (id.HasValue)
            {
                model = new Template5Logic().FetchById(id.Value);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(Template5ViewModel model)
        {
            string userId = User.Identity.GetUserId();
            Template5 template = new Template5();
            template.Name = model.Name;
            model.Date = template.Date = Convert.ToDateTime(model.DateStr) + DateTime.Now.TimeOfDay;
            template.Location = model.Location;
            template.CoordinatorName = model.CoordinatorName;
            template.Activity = model.Activity;
            template.NoOfLeaders = model.NoOfLeaders;
            template.RiskAnalysis = model.RiskAnalysis;
            template.CasualFactorEnvironment = model.CasualFactorEnvironment;
            template.CasualFactorEquipment = model.CasualFactorEquipment;
            template.CasualFactorPeople = model.CasualFactorPeople;
            template.NormalOperationPeople = model.NormalOperationPeople;
            template.NormalOperationEquipment = model.NormalOperationEquipment;
            template.NormalOperationEnvironment = model.NormalOperationEnvironment;
            template.EmergencyEnvironment = model.EmergencyEnvironment;
            template.EmergencyEquipment = model.EmergencyEquipment;
            template.EmergencyPeople = model.EmergencyPeople;
            template.SkillsRequiredByLeaders = model.SkillsRequiredByLeaders;
            template.FormCompletedBy = model.FormCompletedBy;
            template.FormCompletionDate = model.FormCompletionDate = Convert.ToDateTime(model.FormCompletionDateStr) + DateTime.Now.TimeOfDay;
            template.ActivityStatus = model.ActivityStatus;
            template.ApprovedBy = model.ApprovedBy;
            template.Position = model.Position;
            template.ApprovedDate = model.ApprovedDate = Convert.ToDateTime(model.ApprovedDateStr) + DateTime.Now.TimeOfDay;
            template.CreatedBy = userId;
            template.CreatedDate = DateTime.Now;

            if (model.Id > 0)
            {
                template.Id = model.Id;
                template.ModifiedDate = DateTime.Now;
                template.ModifiedBy = userId;
            }
            ViewAsPdf pdf = new Rotativa.PartialViewAsPdf("_Template5Report", model)
            {
                FileName = string.Format("{0}.pdf", Guid.NewGuid()),
                CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
            };
            template.FileName = pdf.FileName;
            string message = new Template5Logic().SaveUpdate(template,model.type);


            byte[] pdfData = pdf.BuildFile(ControllerContext);
            string fullPath = Path.Combine(Server.MapPath("~/Reports"), pdf.FileName);
            using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            {
                fileStream.Write(pdfData, 0, pdfData.Length);
            }
            //var user = new Common().GetById(userId);
            //string body = "Hello " + String.Format("{0} {1}", user.FirstName, user.LastName) + ", <br/><br />";
            //body += model.EmailBody;
            //if (new Common().SendEmail(model.Email, model.Subject, body, fullPath, model.Cc))
            //{

            //}
            return Redirect("~/templatehistory/Created");

        }

        public ActionResult Download(int? id)
        {
            string fileName = new Template5Logic().FetchFile(id.Value);
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Reports/" + fileName));
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}