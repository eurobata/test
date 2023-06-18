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
    public class DomesticSmokeAlarmController : Controller
    {
        public TempletDbContext _context { get; set; }
        public
            DomesticSmokeAlarmController()
        {
            _context = new TempletDbContext();
        }
        // GET: DomesticSmokeAlarm
        public ActionResult Generate(int? id, string prev)
        {
            DomesticSmokeAlarm domesticSmokeAlarm= new DomesticSmokeAlarm();
            if (id.HasValue)
            {
                var mo = _context.DomesticSmokeAlarms.Find(id.Value);
                if (mo != null)
                {
                    domesticSmokeAlarm = mo;

                    domesticSmokeAlarm.datestr = string.Format("{0:yyyy-MM-dd}", mo.Date);
                    domesticSmokeAlarm.InspectionDateStr = string.Format("{0:yyyy-MM-dd}", mo.InspectionDate);
                    


                }
                else
                {
                    domesticSmokeAlarm.datestr = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                    domesticSmokeAlarm.InspectionDateStr = string.Format("{0:yyyy-MM-dd}", DateTime.Now);

                }
            }
            var manger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var CurrentUser = manger.FindById(User.Identity.GetUserId());
            domesticSmokeAlarm.logo = CurrentUser.ProfilePicture;

            return View(domesticSmokeAlarm);
        }
        [HttpPost]
        public ActionResult index(DomesticSmokeAlarm model
            )
        {
          

            model.Date = DateTime.Now;
            if (!string.IsNullOrEmpty(model.InspectionDateStr))
            {
                model.InspectionDate = Convert.ToDateTime(model.InspectionDateStr) + DateTime.Now.TimeOfDay;

            }
            else
            {
                model.InspectionDate = DateTime.Now;
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
                    _context.DomesticSmokeAlarms.Add(model);
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
                _context.DomesticSmokeAlarms.Add(model);
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
            var file = _context.DomesticSmokeAlarms.Find(id.Value);
            if (file != null)
            {
                _file = file.FileName;
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Reports/" + _file)); ;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, _file);
        }


    }
}