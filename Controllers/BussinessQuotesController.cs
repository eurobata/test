using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
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

    public class BussinessQuotesController : Controller
    {
        // GET: BussinessQuotes
        // GET: PressureWashing
        public ActionResult Generate(int? id,string prev)
        {

            ViewBag.pre = prev;
            var manger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var CurrentUser = manger.FindById(User.Identity.GetUserId());
            ViewBag.ProfilePicture = CurrentUser.ProfilePicture;

            Models.BussinessQuotesViewModel model = new Models.BussinessQuotesViewModel();
            if (id.HasValue)
            {

                model = new BussinessQoutesLogic().FetchById(id.Value);
            }
            else

            {
                model.logo = CurrentUser.ProfilePicture;
                model.issueDatestr = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                model.ExpiryDateStr = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                model.ECDStr = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                model.ECoDStr = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                model.issueDatestr = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                model.items.Add(new bussinessQuotesItems());
                model.items.Add(new bussinessQuotesItems());
                model.items.Add(new bussinessQuotesItems());

            
            }

            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Models.BussinessQuotesViewModel model)
        {
            string userId = User.Identity.GetUserId();
            DAL.BussinessQuotesViewModel bussinessQuote = new DAL.BussinessQuotesViewModel();
            List<DAL.bussinessQuotesItem> lstDetails = new List<bussinessQuotesItem>();

            bussinessQuote.createdAt = DateTime.Now;
            bussinessQuote.CreatedBy = userId;
            foreach (var item in model.items)
            {
                lstDetails.Add(new bussinessQuotesItem()
                {
                    item = item.item,
                    
                    ammount = item.ammount,

                });
            }
            if (model.Id > 0)
            {
                bussinessQuote.Id = model.Id;
                bussinessQuote.ModifiedDate = DateTime.Now;
                bussinessQuote.ModifiedBy = userId;
            }
            bussinessQuote.bussinessQuotesItems = lstDetails;
            var manger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var CurrentUser = manger.FindById(User.Identity.GetUserId());
            model.logo = CurrentUser.ProfilePicture;

            ViewAsPdf pdf = new Rotativa.PartialViewAsPdf("_BussinessQuotes", model)
            {
                FileName = string.Format("{0}.pdf", Guid.NewGuid()),
                CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
            };
            bussinessQuote.Filename = pdf.FileName;
            bussinessQuote.Subject = model.Subject;
            bussinessQuote.EmailBody = model.EmailBody;
            bussinessQuote.Email = model.Email;
            bussinessQuote.CC = model.CC;
            bussinessQuote.expirydate = Convert.ToDateTime(model.ExpiryDateStr) + DateTime.Now.TimeOfDay;
            bussinessQuote.issuedate = Convert.ToDateTime(model.issueDatestr) + DateTime.Now.TimeOfDay;
            bussinessQuote.ecd = Convert.ToDateTime(model.ECDStr) + DateTime.Now.TimeOfDay;
            bussinessQuote.ecod = Convert.ToDateTime(model.ECoDStr) + DateTime.Now.TimeOfDay;

            bussinessQuote.Name = model.Name;
            bussinessQuote.address1 = model.address1;
            bussinessQuote.address2 = model.address2;
            bussinessQuote.discount = model.discount;
            bussinessQuote.quoteTxt = model.quoteTxt;
            bussinessQuote.shipingHandling = model.shipingHandling;
            bussinessQuote.totallTaxAmount = model.totallTaxAmount;

            bussinessQuote.subtotall = model.subtotall;
            bussinessQuote.tax = model.tax;
            bussinessQuote.totall = model.totall;
            
            bussinessQuote.phone = model.phone;

            string message = new BussinessQoutesLogic().SaveUpdate(bussinessQuote, model.type);



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
            string fileName = new BussinessQoutesLogic().FetchFile(id.Value);
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Reports/" + fileName));
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}