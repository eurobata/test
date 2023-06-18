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
    public class CustomerTempleteController : Controller
    {
        // GET: CustomerTemplete
        public ActionResult Generate(int? id, string prev)
        {

            ViewBag.pre = prev;
            var manger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var CurrentUser = manger.FindById(User.Identity.GetUserId());
            ViewBag.ProfilePicture = CurrentUser.ProfilePicture;

            Models.CustomerTempleteModel model = new Models.CustomerTempleteModel();
            if (id.HasValue)
            {

                model = new CustomerTempleteLogic().FetchById(id.Value);
            }
            else

            {


                model.UserName = CurrentUser.FirstName + " " + CurrentUser.LastName;
                model.userAdress = CurrentUser.Address+" ," +CurrentUser.city;
                model.userCountry = CurrentUser.Country;
                model.useroffice = CurrentUser.officeNo;
                model.userPhone = CurrentUser.PhoneNumber;
                model.userPostal = CurrentUser.postcode;
                model.userProfile = CurrentUser.ProfilePicture;
                model.useremail = CurrentUser.Email;
           





                model.item.Add(new customerItems());
                model.item.Add(new customerItems());
                model.item.Add(new customerItems());

                

            }
            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Models.CustomerTempleteModel model)
        {
            var manger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var CurrentUser = manger.FindById(User.Identity.GetUserId());

            string userId = User.Identity.GetUserId();
            DAL.CustomerTempleteModel customerModel = new DAL.CustomerTempleteModel();
            List<DAL.customerItem> lstDetails = new List<customerItem>();

            customerModel.createdAt = DateTime.Now;
            customerModel.CreatedBy = userId;
            foreach (var item in model.item)
            {
                lstDetails.Add(new customerItem()
                {
                    service = item.service,
                    price = item.price,
                    quantity = item.quantity,
                    totall = item.totall,
                    decription = item.decription,
                });
            }
            if (model.Id > 0)
            {
                customerModel.Id = model.Id;
                customerModel.ModifiedDate = DateTime.Now;
                customerModel.ModifiedBy = userId;
            }
            model.UserName = CurrentUser.FirstName + " " + CurrentUser.LastName;
            model.userAdress = CurrentUser.Address + " ," + CurrentUser.city;
            model.userCountry = CurrentUser.Country;
            model.useroffice = CurrentUser.officeNo;
            model.userPhone = CurrentUser.PhoneNumber;
            model.userPostal = CurrentUser.postcode;
            model.userProfile = CurrentUser.ProfilePicture;
            model.useremail = CurrentUser.Email;

            customerModel.customerItems = lstDetails;
            ViewAsPdf pdf = new Rotativa.PartialViewAsPdf("CustomerTemp", model)
            {
                FileName = string.Format("{0}.pdf", Guid.NewGuid()),
                CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
            };
            customerModel.Subject = model.Subject;
            customerModel.EmailBody = model.EmailBody;
            customerModel.Email = model.Email;
            customerModel.CC = model.CC;
            customerModel.CustomerName = model.CustomerName;
            customerModel.CustomerAddress = model.CustomerAddress;
            customerModel.userAdress = model.userAdress;
            customerModel.userPhone = model.userPhone;
            customerModel.userProfile = model.userProfile;
            customerModel.useroffice = model.useroffice;
            customerModel.useremail = model.useremail;
            customerModel.userCountry = model.userCountry;
            customerModel.userPostal = model.userPostal;
            customerModel.dscount = model.dscount;
            customerModel.totall = model.totall;
            customerModel.subtotall = model.subtotall;


            string message = new CustomerTempleteLogic().SaveUpdate(customerModel, model.type);



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
            string fileName = new CustomerTempleteLogic().FetchFile(id.Value);
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Reports/" + fileName));
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}