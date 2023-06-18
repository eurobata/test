using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TemplateManagementSystem.Models;
using Rotativa;
using System.IO;

namespace TemplateManagementSystem.Controllers
{
    [Authorize]
    public class Covid19Controller : Controller
    {
        public TempletDbContext _context { get; set; }
        public 
            Covid19Controller()
        {
            _context = new TempletDbContext();
        }
        // GET: Covid19
        public ActionResult generate(int? id, string prev)
        {
            covid19 covid19 = new covid19();

           
            if (id.HasValue)
            {
                var mo = _context.Covid19s.Find(id.Value);
                if (mo != null)
                {
                    covid19 = mo;

                    covid19.datestr = string.Format("{0:yyyy-MM-dd}", mo.Date);
                    covid19.datestr2 = string.Format("{0:yyyy-MM-dd}", mo.Date2);
                   


                }
                else
                {
                    covid19.datestr = string.Format("{0:yyyy-MM-dd}",DateTime.Now);
                    covid19.datestr2 = string.Format("{0:yyyy-MM-dd}",DateTime.Now);

                }
            }
            var manger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var CurrentUser = manger.FindById(User.Identity.GetUserId());
            covid19.logo = CurrentUser.ProfilePicture;
            covid19.locationLin1 = CurrentUser.Address;
            covid19.email = CurrentUser.Email;
            covid19.phone = CurrentUser.PhoneNumber;
            covid19.city = CurrentUser.city;
            covid19.postcode = CurrentUser.postcode;
            covid19.state = CurrentUser.Country;



            return View(covid19);
        }


        [HttpPost]
        public ActionResult index(covid19 model
            )
        {
            var manger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var CurrentUser = manger.FindById(User.Identity.GetUserId());
            model.logo = CurrentUser.ProfilePicture;
            model.locationLin1 = CurrentUser.Address;
            model.email = CurrentUser.Email;
            model.phone = CurrentUser.PhoneNumber;
            model.city = CurrentUser.city;
            model.postcode = CurrentUser.postcode;
            model.state = CurrentUser.Country;
            if (!string.IsNullOrEmpty(model.datestr))
            {
                model.Date = Convert.ToDateTime(model.datestr) + DateTime.Now.TimeOfDay;

            }
            else
            {
                model.Date = DateTime.Now;
            }
            if (!string.IsNullOrEmpty(model.datestr2))
            {
                model.Date2 = Convert.ToDateTime(model.datestr2) + DateTime.Now.TimeOfDay;

            }
            else
            {
                model.Date2 = DateTime.Now;
            }
            
            ViewAsPdf pdf = new Rotativa.PartialViewAsPdf("_generate", model)
            {
                FileName = string.Format("{0}.pdf", Guid.NewGuid()),
                CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
            };
            model.FileName = pdf.FileName;
            model.ModifiedBy = User.Identity.GetUserId();

            if (model.Id > 0)
            {
                if (model.type == "Copy")
                {
                    _context.Covid19s.Add(model);
                    _context.SaveChanges();

                }
                else
                {

                    model.ModifiedDate = DateTime.Now;
                    _context.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                }




            }
            else
            {
                _context.Covid19s.Add(model);
                _context.SaveChanges();

            }
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
            string _file = "";
            var file = _context.Covid19s.Find(id.Value);
            if (file != null)
            {
                _file = file.FileName;
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Reports/" + _file)); ;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, _file);
        }

    }
}