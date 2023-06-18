using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TemplateManagementSystem.Models;
using System.IO;
using Rotativa;
using TemplateManagementSystem.BLL;
using TemplateManagementSystem.DAL;

namespace TemplateManagementSystem.Controllers
{
    [Authorize]
    public class cleaningChecklistController : Controller
    {
        public TempletDbContext _context { get; set; }
        public cleaningChecklistController() {
        _context=new TempletDbContext();
        }
        public cleaningChecklistController(TempletDbContext context)
        {
            _context = context;
        }


        // GET: cleaningChecklist
        public ActionResult generate(int? id, string prev)
        {

            CleaningCheckList job = new CleaningCheckList();

            var manger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var CurrentUser = manger.FindById(User.Identity.GetUserId());
            job.logo = CurrentUser.ProfilePicture;
            if (id.HasValue)
            {
                var mo = _context.CleaningCheckLists.Find(id.Value);
                if(mo != null)
                {
                    job = mo;

                    job.datestr = string.Format("{0:yyyy-MM-dd}", mo.date);
                    

                }
                else
                {
                    job.datestr = string.Format("{0:yyyy-MM-dd}", DateTime.Now);

                }
            }
           

            return View(job);







        }
        [HttpPost]
        public ActionResult index(CleaningCheckList model,HttpPostedFileBase afterPhoto)
        {
            if(afterPhoto != null)
            {
                if (afterPhoto.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(afterPhoto.FileName);
                    string _path = Path.Combine(Server.MapPath("~/pics"), _FileName);
                    afterPhoto.SaveAs(_path);
                    model.afterPhoto = _FileName;
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
                    _context.CleaningCheckLists.Add(model);
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
                _context.CleaningCheckLists.Add(model);
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
            var file = _context.CleaningCheckLists.Find(id.Value);
            if(file != null)
            {
                _file = file.FileName;
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Reports/" + _file)); ;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, _file);
        }

    }
}