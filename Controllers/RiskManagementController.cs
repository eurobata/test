﻿using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Rotativa;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TemplateManagementSystem.Models;

namespace TemplateManagementSystem.Controllers
{
    [Authorize]
    public class RiskManagementController : Controller
    {

        public TempletDbContext _context { get; set; }
        public
            RiskManagementController()
        {
            _context = new TempletDbContext();
        }
        // GET: RiskManagement
        public ActionResult Generate(int? id, string prev)
        {

            RiskManagement risk = new RiskManagement();

            if (id.HasValue)
            {
                var mo = _context.RiskManagements.Find(id.Value);
                if (mo != null)
                {
                    risk = mo;

                    risk.datestr = string.Format("{0:yyyy-MM-dd}", mo.Date);
                    


                }
                else
                {
                    risk.datestr = string.Format("{0:yyyy-MM-dd}", DateTime.Now);

                }
            }
            var manger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var CurrentUser = manger.FindById(User.Identity.GetUserId());
            risk.logo = CurrentUser.ProfilePicture;
            risk.addressStr = CurrentUser.Address;
          
            return View(risk);
        }
        [HttpPost]
        public ActionResult index(RiskManagement model
          )
        {
            var manger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var CurrentUser = manger.FindById(User.Identity.GetUserId());
            model.logo = CurrentUser.ProfilePicture;
            model.addressStr = CurrentUser.Address;
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
                if (model.type == "Copy")
                {
                    _context.RiskManagements.Add(model);
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
                _context.RiskManagements.Add(model);
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
            var file = _context.RiskManagements.Find(id.Value);
            if (file != null)
            {
                _file = file.FileName;
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Reports/" + _file)); ;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, _file);
        }

    }
}