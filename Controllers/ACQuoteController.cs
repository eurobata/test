using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using TemplateManagementSystem.Models;

namespace TemplateManagementSystem.Controllers
{
    [Authorize]
    public class ACQuoteController : Controller
    {
        // GET: ACQuote
        public TempletDbContext _context { get; set; }
        public ACQuoteController()
        {
            _context = new TempletDbContext();
        }
        public ACQuoteController(TempletDbContext context)
        {
            _context = context;
        }

        public ActionResult generate(int? id, string prev)
        {
            ACQuote aCQuote = new ACQuote();
            var manger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var CurrentUser = manger.FindById(User.Identity.GetUserId());
            aCQuote.logo = CurrentUser.ProfilePicture;
            if (id.HasValue)
            {
                var mo = _context.ACQuotes.Find(id.Value);
                if (mo != null)
                {
                    aCQuote = mo;

                    aCQuote.datestr = string.Format("{0:yyyy-MM-dd}", mo.Date);

                }
                else
                {
                    aCQuote.datestr = string.Format("{0:yyyy-MM-dd}", DateTime.Now);


                }
            }
            return View(aCQuote);
        }
        public string saveImage(HttpPostedFileBase file, string variable)
        {
            if(file == null)
            {
                return variable;
            }
            if ( file.ContentLength > 0)
            {
                string _FileName = Path.GetFileName(file.FileName);
                string _path = Path.Combine(Server.MapPath("~/pics"), _FileName);
                file.SaveAs(_path);
                variable= _FileName;
            }
            return variable;
        }
        [HttpPost]
        public ActionResult index(ACQuote model, HttpPostedFileBase image1, HttpPostedFileBase image2, HttpPostedFileBase image3, HttpPostedFileBase image4, HttpPostedFileBase image5, HttpPostedFileBase image6)
        {
            model.image1=  saveImage(image1, model.image1);
            model.image2 = saveImage(image2, model.image2);
            model.image3 = saveImage(image3, model.image3);
            model.image4 = saveImage(image4, model.image4);
            model.image5 = saveImage(image5, model.image5);
            model.image6 = saveImage(image6, model.image6);

            if (!string.IsNullOrEmpty(model.datestr))
            {
                model.Date = Convert.ToDateTime(model.datestr) + DateTime.Now.TimeOfDay;

            }
            else
            {
                model.Date = DateTime.Now;
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
                if(model.type== "Copy")
                {
                    _context.ACQuotes.Add(model);
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
                
                try
                {
                    // Your code...
                    // Could also be before try if you know the exception occurs in SaveChanges
                    _context.ACQuotes.Add(model);
                    _context.SaveChanges();

                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
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
            var file = _context.ACQuotes.Find(id.Value);
            if (file != null)
            {
                _file = file.FileName;
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Reports/" + _file)); ;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, _file);
        }

    }
}