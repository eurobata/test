using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TemplateManagementSystem.DAL;
using TemplateManagementSystem.Models;

namespace TemplateManagementSystem.BLL
{
    public class Template5Logic
    {
        TemplateManagementSystemEntities db = new TemplateManagementSystemEntities();
        public Template5ViewModel FetchById(int id)
        {
            Template5ViewModel model = new Template5ViewModel();
            var template = db.Template5.Where(s => s.Id == id).FirstOrDefault();
            if (template != null)
            {
                model.Id = template.Id;
                model.Name = template.Name;
                model.Date = template.Date;
                model.DateStr = string.Format("{0:yyyy-MM-dd}", template.Date);
                model.Location = template.Location;
                model.CoordinatorName = template.CoordinatorName;
                model.Activity = template.Activity;
                model.NoOfLeaders = template.NoOfLeaders;
                model.RiskAnalysis = template.RiskAnalysis;
                model.CasualFactorPeople = template.CasualFactorPeople;
                model.CasualFactorEquipment = template.CasualFactorEquipment;
                model.CasualFactorEnvironment = template.CasualFactorEnvironment;
                model.NormalOperationPeople = template.NormalOperationPeople;
                model.NormalOperationEquipment = template.NormalOperationEquipment;
                model.NormalOperationEnvironment = template.NormalOperationEnvironment;
                model.EmergencyPeople = template.EmergencyPeople;
                model.EmergencyEquipment = template.EmergencyEquipment;
                model.EmergencyEnvironment = template.EmergencyEnvironment;
                model.SkillsRequiredByLeaders = template.SkillsRequiredByLeaders;
                model.FormCompletedBy = template.FormCompletedBy;
                model.FormCompletionDate = template.FormCompletionDate;
                model.FormCompletionDateStr = string.Format("{0:yyyy-MM-dd}", template.FormCompletionDate);
                model.Position = template.Position;
                model.ActivityStatus = template.ActivityStatus;
                model.ApprovedBy = template.ApprovedBy;
                model.ApprovedDate = template.ApprovedDate;
                model.ApprovedDateStr = string.Format("{0:yyyy-MM-dd}", template.ApprovedDate);
            }
            return model;
        }

        public string SaveUpdate(Template5 model, string method)
        {
            string message = "";
            try
            {
                if (model.Id > 0)
                {
                    if (method == "Copy")
                    {
                        db.Template5.Add(model);
                        db.SaveChanges();
                        message = "Success: Data has been saved successfully.";

                    }
                    else
                    {
                        Template5 oldTemplate = db.Template5.Where(s => s.Id == model.Id).FirstOrDefault();
                        if (oldTemplate != null)
                        {
                            oldTemplate.Name = model.Name;
                            oldTemplate.Date = model.Date;
                            oldTemplate.ModifiedDate = model.ModifiedDate;
                            oldTemplate.ModifiedBy = model.ModifiedBy;
                            oldTemplate.Location = model.Location;
                            oldTemplate.CoordinatorName = model.CoordinatorName;
                            oldTemplate.Activity = model.Activity;
                            oldTemplate.NoOfLeaders = model.NoOfLeaders;
                            oldTemplate.RiskAnalysis = model.RiskAnalysis;
                            oldTemplate.CasualFactorPeople = model.CasualFactorPeople;
                            oldTemplate.CasualFactorEquipment = model.CasualFactorEquipment;
                            oldTemplate.CasualFactorEnvironment = model.CasualFactorEnvironment;
                            oldTemplate.NormalOperationPeople = model.NormalOperationPeople;
                            oldTemplate.NormalOperationEquipment = model.NormalOperationEquipment;
                            oldTemplate.NormalOperationEnvironment = model.NormalOperationEnvironment;
                            oldTemplate.EmergencyPeople = model.EmergencyPeople;
                            oldTemplate.EmergencyEquipment = model.EmergencyEquipment;
                            oldTemplate.EmergencyEnvironment = model.EmergencyEnvironment;
                            oldTemplate.SkillsRequiredByLeaders = model.SkillsRequiredByLeaders;
                            oldTemplate.FormCompletedBy = model.FormCompletedBy;
                            oldTemplate.FormCompletionDate = model.FormCompletionDate;
                            oldTemplate.ActivityStatus = model.ActivityStatus;
                            oldTemplate.ApprovedBy = model.ApprovedBy;
                            oldTemplate.Position = model.Position;
                            oldTemplate.ApprovedDate = model.ApprovedDate;
                            oldTemplate.FileName = model.FileName;

                            db.SaveChanges();
                            message = "Success: Data has been updated successfully.";
                        }
                    }
                   
                }
                else
                {
                    db.Template5.Add(model);
                    db.SaveChanges();
                    message = "Success: Data has been saved successfully.";
                }
            }
            catch (Exception ex)
            {
                message = "Error: " + ex.Message;
            }
            return message;
        }

        public string FetchFile(int id)
        {
            string fileName = "";
            var report = db.Template5.Where(s => s.Id == id).FirstOrDefault();
            if (report != null)
            {
                fileName = report.FileName;
            }
            return fileName;
        }
    }
}