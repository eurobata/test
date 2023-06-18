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
using System.EnterpriseServices;

namespace TemplateManagementSystem.Controllers
{
    [Authorize]
    public class boilerServiceController : Controller
    {

        public TempletDbContext _context { get; set; }
        public
           boilerServiceController()
        {
            _context = new TempletDbContext();
        }
        // GET: boilerService
        public ActionResult generate(int? id, string prev)
        {
            ServiceBoiler risk = new ServiceBoiler();

            if (id.HasValue)
            {
                var mo = _context.ServiceBoilers.Find(id.Value);
                if (mo != null)
                {
                    risk = mo;

                    risk.datestr = string.Format("{0:yyyy-MM-dd}", mo.date);
                    risk.duDatestr = string.Format("{0:yyyy-MM-dd}", mo.Duedate);




                }
                else
                {
                    risk.datestr = string.Format("{0:yyyy-MM-dd}",DateTime.Now);
                    risk.duDatestr = string.Format("{0:yyyy-MM-dd}", DateTime.Now);



                }
            }
            var manger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var CurrentUser = manger.FindById(User.Identity.GetUserId());
            risk.logo = CurrentUser.ProfilePicture;
            

            return View(risk);
        }
       
        [HttpPost]
        public ActionResult myGenerate(TemplateManagementSystem.Models.ServiceBoiler modal,string BussinessDetail)
        {

            if (!string.IsNullOrEmpty(modal.datestr))
            {
                modal.date = Convert.ToDateTime(modal.datestr) + DateTime.Now.TimeOfDay;

            }
            else
            {
                modal.date = DateTime.Now;
            }
            if (!string.IsNullOrEmpty(modal.duDatestr))
            {
                modal.Duedate = Convert.ToDateTime(modal.duDatestr) + DateTime.Now.TimeOfDay;

            }
            else
            {
                modal.Duedate = DateTime.Now;
            }


            
            ViewAsPdf pdf = new Rotativa.PartialViewAsPdf("_generate", modal)
            {
                FileName = string.Format("{0}.pdf", Guid.NewGuid()),
                CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
            };
            modal.FileName = pdf.FileName;
            modal.ModifiedBy = User.Identity.GetUserId();

            if (modal.Id > 0)
            {
                if (modal.type == "Copy")
                {
                    _context.ServiceBoilers.Add(modal);
                    _context.SaveChanges();

                }
                else
                {

                    modal.ModifiedDate = DateTime.Now;
                    _context.Entry(modal).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                }




            }
            else
            {
                _context.ServiceBoilers.Add(modal);
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
            //body += modal.EmailBody;
            //if (new Common().SendEmail(modal.Email, modal.Subject, body, fullPath,modal.Cc))
            //{

            //}
            return Redirect("~/templatehistory/Created");


        }


        public ActionResult Download(int? id)
        {
            string _file = "";
            var file = _context.ServiceBoilers.Find(id.Value);
            if (file != null)
            {
                _file = file.FileName;
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Reports/" + _file)); ;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, _file);
        }

    
    }
}