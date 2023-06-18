using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TemplateManagementSystem.DAL;
using TemplateManagementSystem.Models;

namespace TemplateManagementSystem.BLL
{
    [Authorize]

    public class WashingTempleteLogic
    {
        TemplateManagementSystemEntities db = new TemplateManagementSystemEntities();

        public Models.PressureWashingViewModel FetchById(int id)
        {
          Models.PressureWashingViewModel model = new  Models.PressureWashingViewModel();
          DAL. PressureWashingViewModel dbmodel = db.PressureWashingViewModels.Where(s => s.Id == id).FirstOrDefault();
            if (dbmodel != null)
            {
                model.Id = dbmodel.Id;
                model.Subject = dbmodel.Subject;
                model.EmailBody = dbmodel.EmailBody;
                model.Email = dbmodel.Email;
                model.CC= dbmodel.CC;
                model.date = dbmodel.date;
                model.datetimestring= string.Format("{0:yyyy-MM-dd}", dbmodel.date);


                model.Notes = dbmodel.Notes;
                model.InvoiceId = dbmodel.InvoiceId;
                model.For = dbmodel.For;
                model.Address= dbmodel.Address;
                model.subtotall= dbmodel.subtotall;
                model.tax = dbmodel.tax;
                model.totall = dbmodel.totall;
                model.PreparedBy= dbmodel.PreparedBy;
              


                foreach (var item in dbmodel.WashingTempletItems)
                {
                    model.templetItems.Add(new WashingTempletItems
                    {
                        id = item.id,
                        item = item.item,
                        price = item.price,
                       quantity = item.quantity,
                        ammount = item.ammount,
                        description = item.description,

                    });
                }
            }
            return model;
        }

        public string SaveUpdate(DAL.PressureWashingViewModel model, string method)
          {
            string message = "";
            try
            {
                if (model.Id > 0)
                {
                    if (method == "Copy")
                    {
                        db.PressureWashingViewModels.Add(model);
                        db.SaveChanges();
                        message = "Success: Data has been saved successfully.";

                    }
                    else
                    {

                        DAL.PressureWashingViewModel oldSheet = db.PressureWashingViewModels.Where(s => s.Id == model.Id).FirstOrDefault();
                        if (oldSheet != null)
                        {
                            oldSheet.Subject = model.Subject;
                            oldSheet.EmailBody = model.EmailBody;
                            oldSheet.Email = model.Email;
                            oldSheet.CC = model.CC;
                            oldSheet.date = model.date;
                            oldSheet.Notes = model.Notes;
                            oldSheet.InvoiceId = model.InvoiceId;
                            oldSheet.For = model.For;
                            oldSheet.Address = model.Address;
                            oldSheet.subtotall = model.subtotall;
                            oldSheet.tax = model.tax;
                            oldSheet.totall = model.totall;
                            oldSheet.PreparedBy = model.PreparedBy;

                          
                            db.WashingTempletItems.RemoveRange(oldSheet.WashingTempletItems);

                            oldSheet.WashingTempletItems = model.WashingTempletItems;
                            db.SaveChanges();
                            message = "Success: Data has been updated successfully.";
                        }
                    }

                }
                else
                {
                    db.PressureWashingViewModels.Add(model);
                    db.SaveChanges();
                    message = "Success: Data has been saved successfully.";

                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }
            }
            return message;
        }
        public string FetchFile(int id)
        {
            string fileName = "";
            var report = db.PressureWashingViewModels.Where(s => s.Id == id).FirstOrDefault();
            if (report != null)
            {
                fileName = report.Filename;
            }
            return fileName;
        }

        //public string FetchFile(int id)
        //{
        //    string fileName = "";
        //    var report = db.Dailydbmodels.Where(s => s.Id == id).FirstOrDefault();
        //    if (report != null)
        //    {
        //        fileName = report.FileName;
        //    }
        //    return fileName;
        //}


    }
}