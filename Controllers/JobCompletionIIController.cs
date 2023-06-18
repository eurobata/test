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
    public class JobCompletionIIController : Controller
    {
        // GET: JobCompletionII
        public TempletDbContext _context { get; set; }
        public
            JobCompletionIIController()
        {
            _context = new TempletDbContext();
        }
        // GET: JobCompletion
        public ActionResult generate(int? id, string prev)
        {
            JobCompletionII job = new JobCompletionII();

            if (id.HasValue)
            {
                var mo = _context.JobCompletionIIs.Find(id.Value);
                if (mo != null)
                {
                    job = mo;

                    job.datestr = string.Format("{0:yyyy-MM-dd}", mo.date);




                }
                else
                {
                    job.datestr = string.Format("{0:yyyy-MM-dd}", DateTime.Now);


                }
            }
            var manger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var CurrentUser = manger.FindById(User.Identity.GetUserId());
            job.logo = CurrentUser.ProfilePicture;
            job.locationLin1 = CurrentUser.Address;
            job.email = CurrentUser.Email;
            job.phone = CurrentUser.PhoneNumber;
            job.city = CurrentUser.city;
            job.postcode = CurrentUser.postcode;
            job.state = CurrentUser.Country;
            job.Name = CurrentUser.FirstName + " " + CurrentUser.LastName;



            return View(job);
        }
        [HttpPost]
        public ActionResult index(JobCompletionII model, HttpPostedFileBase beforPhoto, HttpPostedFileBase afterPhoto
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
            model.Name = CurrentUser.FirstName + " " + CurrentUser.LastName;



            if (afterPhoto != null)
            {
                if (afterPhoto.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(afterPhoto.FileName);
                    string _path = Path.Combine(Server.MapPath("~/pics"), _FileName);
                    afterPhoto.SaveAs(_path);
                    model.afterPhoto = _FileName;
                }
            }
            if (beforPhoto != null)
            {
                if (beforPhoto.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(beforPhoto.FileName);
                    string _path = Path.Combine(Server.MapPath("~/pics"), _FileName);
                    beforPhoto.SaveAs(_path);
                    model.beforPhoto = _FileName;
                }
            }

            if (!string.IsNullOrEmpty(model.datestr))
            {
                model.date = Convert.ToDateTime(model.datestr) + DateTime.Now.TimeOfDay;

            }
            else
            {
                model.date = DateTime.Now;
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
                    _context.JobCompletionIIs.Add(model);
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
                _context.JobCompletionIIs.Add(model);
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
            var file = _context.JobCompletionIIs.Find(id.Value);
            if (file != null)
            {
                _file = file.FileName;
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Reports/" + _file)); ;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, _file);
        }


    }

}