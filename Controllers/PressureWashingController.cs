using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TemplateManagementSystem.BLL;
using TemplateManagementSystem.Models;
using Rotativa;
using System.IO;
using TemplateManagementSystem.DAL;
using System.Data.Entity.Infrastructure;

namespace TemplateManagementSystem.Controllers
{
    public class PressureWashingController : Controller
    {
        // GET: PressureWashing
        public ActionResult Generate(int? id, string prev)
        {

            ViewBag.pre = prev;
            var manger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var CurrentUser = manger.FindById(User.Identity.GetUserId());
            ViewBag.ProfilePicture = CurrentUser.ProfilePicture;

            Models. PressureWashingViewModel model = new Models.PressureWashingViewModel();
            if (id.HasValue)
            {
               
                model = new WashingTempleteLogic().FetchById(id.Value);
            }
            else

            {
                model.datetimestring = string.Format("{0:yyyy-MM-dd}",DateTime.Now);
                model.templetItems.Add(new  WashingTempletItems());
                model.templetItems.Add(new  WashingTempletItems());
                model.templetItems.Add(new  WashingTempletItems());
                model.userProfile = CurrentUser.ProfilePicture;
            }
            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Models.PressureWashingViewModel model)
        {
            var manger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var CurrentUser = manger.FindById(User.Identity.GetUserId());

            string userId = User.Identity.GetUserId();
            DAL.PressureWashingViewModel washingModel = new DAL.PressureWashingViewModel();
            List<DAL.WashingTempletItem> lstDetails = new List<WashingTempletItem>();

            washingModel.date = model.date;
            washingModel.For = model.For;
            washingModel.createdAt = DateTime.Now;
            washingModel.CreatedBy = userId;
            foreach (var item in model.templetItems)
            {
                lstDetails.Add(new WashingTempletItem()
                {
                    item = item.item,
                    description = item.description,
                    price = item.price,
                quantity = item.quantity,
                ammount=item.ammount,

                });
            }
            if (model.Id > 0)
            {
                washingModel.Id = model.Id;
                washingModel.ModifiedDate = DateTime.Now;
                washingModel.ModifiedBy = userId;
            }
            model.userProfile = CurrentUser.ProfilePicture;
            washingModel.WashingTempletItems = lstDetails;
            ViewAsPdf pdf = new Rotativa.PartialViewAsPdf("_DailywashingModel", model)
            {
                FileName = string.Format("{0}.pdf", Guid.NewGuid()),
                CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
            };
            washingModel.Filename = pdf.FileName;
            washingModel.Subject = model.Subject;
            washingModel.EmailBody = model.EmailBody;
            washingModel.Email = model.Email;
            washingModel.CC = model.CC;
            washingModel.date = Convert.ToDateTime(model.datetimestring) + DateTime.Now.TimeOfDay;
            
            washingModel.Notes = model.Notes;
            washingModel.InvoiceId = model.InvoiceId;
            washingModel.For = model.For;
            washingModel.Address = model.Address;
            washingModel.subtotall = model.subtotall;
            washingModel.tax = model.tax;
            washingModel.totall = model.totall;
            washingModel.PreparedBy = model.PreparedBy;
            washingModel.userProfile = model.userProfile;


            string message = new WashingTempleteLogic().SaveUpdate(washingModel, model.type);



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
            return Redirect("~/templatehistory/Created");

        }
        public ActionResult Download(int? id)
        {
            string fileName = new WashingTempleteLogic().FetchFile(id.Value);
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Reports/" + fileName));
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}