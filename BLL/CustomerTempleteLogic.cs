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

    public class CustomerTempleteLogic
    {

        TemplateManagementSystemEntities db = new TemplateManagementSystemEntities();

        public Models.CustomerTempleteModel FetchById(int id)
        {
            Models.CustomerTempleteModel model = new Models.CustomerTempleteModel();
            DAL.CustomerTempleteModel dbmodel = db.CustomerTempleteModels.Where(s => s.Id == id).FirstOrDefault();
            if (dbmodel != null)
            {
                model.Id = dbmodel.Id;
                model.Subject = dbmodel.Subject;
                model.EmailBody = dbmodel.EmailBody;
                model.Email = dbmodel.Email;
                model.CC = dbmodel.CC;
                model.CustomerName = dbmodel.CustomerName;
                model.CustomerAddress = dbmodel.CustomerAddress;
                model.userAdress = dbmodel.userAdress;
                model.userPhone = dbmodel.userPhone;
                model.userProfile = dbmodel.userProfile;
                model.useroffice = dbmodel.useroffice;
                model.useremail = dbmodel.useremail;
                model.userCountry = dbmodel.userCountry;
                model.userPostal = dbmodel.userPostal;
                model.dscount = dbmodel.dscount;
                model.subtotall = dbmodel.subtotall;
                

                model.totall = dbmodel.totall;



                foreach (var item in dbmodel.customerItems)
                {
                    model.item.Add(new customerItems
                    {
                        id = item.id,
                        service=item.service,
                        price=item.price,
                        quantity=item.quantity,
                        totall=item.totall,
                        decription=item.decription,




                    });
                }
            }
            return model;
        }

        public string SaveUpdate(DAL.CustomerTempleteModel model, string method)
        {
            string message = "";
            try
            {
                if (model.Id > 0)
                {
                    if (method == "Copy")
                    {
                        db.CustomerTempleteModels.Add(model);
                        db.SaveChanges();
                        message = "Success: Data has been saved successfully.";

                    }
                    else
                    {

                        DAL.CustomerTempleteModel oldSheet = db.CustomerTempleteModels.Where(s => s.Id == model.Id).FirstOrDefault();
                        if (oldSheet != null)
                        {
                            oldSheet.Subject = model.Subject;
                            oldSheet.EmailBody = model.EmailBody;
                            oldSheet.Email = model.Email;
                            oldSheet.CC = model.CC;
                            oldSheet.CustomerName = model.CustomerName;
                            oldSheet.CustomerAddress = model.CustomerAddress;
                            oldSheet.userAdress = model.userAdress;
                            oldSheet.userPhone = model.userPhone;
                            oldSheet.userProfile = model.userProfile;
                            oldSheet.useroffice = model.useroffice;
                            oldSheet.useremail = model.useremail;
                            oldSheet.userCountry = model.userCountry;
                            oldSheet.userPostal = model.userPostal;
                            oldSheet.dscount = model.dscount;
                            oldSheet.subtotall = model.subtotall;



                            oldSheet.totall = model.totall;

                            db.customerItems.RemoveRange(oldSheet.customerItems);

                            oldSheet.customerItems = model.customerItems;
                            db.SaveChanges();
                            message = "Success: Data has been updated successfully.";
                        }
                    }

                }
                else
                {
                    db.CustomerTempleteModels.Add(model);
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
            var report = db.CustomerTempleteModels.Where(s => s.Id == id).FirstOrDefault();
            if (report != null)
            {
                fileName = report.Filename;
            }
            return fileName;
        }

    }
}