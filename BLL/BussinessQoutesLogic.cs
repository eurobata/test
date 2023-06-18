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

    public class BussinessQoutesLogic
    {

        TemplateManagementSystemEntities db = new TemplateManagementSystemEntities();

        public Models.BussinessQuotesViewModel FetchById(int id)
        {
            Models.BussinessQuotesViewModel model = new Models.BussinessQuotesViewModel();
            DAL.BussinessQuotesViewModel dbmodel = db.BussinessQuotesViewModels.Where(s => s.Id == id).FirstOrDefault();
            if (dbmodel != null)
            {
                model.Id = dbmodel.Id;
                model.Subject = dbmodel.Subject;
                model.EmailBody = dbmodel.EmailBody;
                model.Email = dbmodel.Email;
                model.CC = dbmodel.CC;
                model.expirydate = dbmodel.expirydate;
                model.issuedate = dbmodel.issuedate;

                model.issueDatestr = string.Format("{0:yyyy-MM-dd}", dbmodel.issuedate);
                model.ExpiryDateStr = string.Format("{0:yyyy-MM-dd}", dbmodel.expirydate);

                model.ECDStr = string.Format("{0:yyyy-MM-dd}", dbmodel.ecd);
                model.ECoDStr = string.Format("{0:yyyy-MM-dd}", dbmodel.ecod);
                model.ecd = dbmodel.ecd;
                model.ecod = dbmodel.ecod;
                model.Name = dbmodel.Name;
                model.address1 = dbmodel.address1;
                model.address2 = dbmodel.address2;
                model.phone = dbmodel.phone;
                model.quoteTxt = dbmodel.quoteTxt;
                model.discount=dbmodel.discount;
                model.totallTaxAmount = dbmodel.totallTaxAmount;
                model.shipingHandling = dbmodel.shipingHandling;




                model.subtotall = dbmodel.subtotall;
                model.tax = dbmodel.tax;
                model.totall = dbmodel.totall;
                


                foreach (var item in dbmodel.bussinessQuotesItems)
                {
                    model.items.Add(new bussinessQuotesItems
                    {
                        id = item.id,
                        item = item.item,
                        ammount = item.ammount,
                      
                    });
                }
            }
            return model;
        }

        public string SaveUpdate(DAL.BussinessQuotesViewModel model, string method)
        {
            string message = "";
            try
            {
                if (model.Id > 0)
                {
                    if (method == "Copy")
                    {
                        db.BussinessQuotesViewModels.Add(model);
                        db.SaveChanges();
                        message = "Success: Data has been saved successfully.";

                    }
                    else
                    {

                        DAL.BussinessQuotesViewModel oldSheet = db.BussinessQuotesViewModels.Where(s => s.Id == model.Id).FirstOrDefault();
                        if (oldSheet != null)
                        {
                            oldSheet.Subject = model.Subject;
                            oldSheet.EmailBody = model.EmailBody;
                            oldSheet.Email = model.Email;
                            oldSheet.CC = model.CC;
                            oldSheet.expirydate = model.expirydate;
                            oldSheet.issuedate = model.issuedate;

                            oldSheet.issuedate = model.issuedate;
                            oldSheet.expirydate = model.expirydate;
                            oldSheet.ecd = model.ecd;
                           oldSheet.ecod = model.ecod;
                            oldSheet.Name = model.Name;
                            oldSheet.address1 = model.address1;
                            oldSheet.address2 = model.address2;
                            oldSheet.phone = model.phone;
                            oldSheet.quoteTxt = model.quoteTxt;
                            oldSheet.discount = model.discount;
                            oldSheet.totallTaxAmount = model.totallTaxAmount;
                            oldSheet.shipingHandling = model.shipingHandling;

                            oldSheet.Subject = model.Subject;
                            oldSheet.EmailBody = model.EmailBody;
                            oldSheet.Email = model.Email;
                            oldSheet.CC = model.CC;
                            oldSheet.subtotall = model.subtotall;
                            oldSheet.tax = model.tax;
                            oldSheet.totall = model.totall;
                         

                            db.bussinessQuotesItems.RemoveRange(oldSheet.bussinessQuotesItems);

                            oldSheet.bussinessQuotesItems = model.bussinessQuotesItems;
                            db.SaveChanges();
                            message = "Success: Data has been updated successfully.";
                        }
                    }

                }
                else
                {
                    db.BussinessQuotesViewModels.Add(model);
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
            var report = db.BussinessQuotesViewModels.Where(s => s.Id == id).FirstOrDefault();
            if (report != null)
            {
                fileName = report.Filename;
            }
            return fileName;
        }
    }
}