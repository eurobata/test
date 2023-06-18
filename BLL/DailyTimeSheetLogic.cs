using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TemplateManagementSystem.DAL;
using TemplateManagementSystem.Models;

namespace TemplateManagementSystem.BLL
{
    public class DailyTimeSheetLogic
    {
        
        TemplateManagementSystemEntities db = new TemplateManagementSystemEntities();
        public DailyTimeSheetViewModel FetchById(int id)
        {
            DailyTimeSheetViewModel model = new DailyTimeSheetViewModel();
            DailyTimeSheet timeSheet = db.DailyTimeSheets.Where(s => s.Id == id).FirstOrDefault();
            if (timeSheet != null)
            {
                model.Id = timeSheet.Id;
                model.Department = timeSheet.Department;
                model.Location = timeSheet.Location;
                model.Name = timeSheet.Name;
                foreach (var item in timeSheet.TimeSheetDetails)
                {
                    model.lstDetails.Add(new DailyTimeSheetDetailViewModel
                    {
                        Id = item.Id,
                        Initials = item.Initials,
                        JobDescription = item.JobDescription,
                        TimeStarted = item.TimeStarted,
                        TimeStopped  = item.TimeStopped,
                    });
                }
            }
            return model;
        }

        public string SaveUpdate(DailyTimeSheet model, string method)
        {
            string message = "";
            try
            {
                if (model.Id > 0)
                {
                    if (method == "Copy")
                    {
                        db.DailyTimeSheets.Add(model);
                        db.SaveChanges();
                        message = "Success: Data has been saved successfully.";

                    }
                    else
                    {

                        DailyTimeSheet oldSheet = db.DailyTimeSheets.Where(s => s.Id == model.Id).FirstOrDefault();
                        if (oldSheet != null)
                        {
                            oldSheet.Name = model.Name;
                            oldSheet.Location = model.Location;
                            oldSheet.ModifiedDate = model.ModifiedDate;
                            oldSheet.Department = model.Department;
                            oldSheet.ModifiedBy = model.ModifiedBy;
                            oldSheet.FileName = model.FileName;
                            db.TimeSheetDetails.RemoveRange(oldSheet.TimeSheetDetails);
                            oldSheet.TimeSheetDetails = model.TimeSheetDetails;
                            db.SaveChanges();
                            message = "Success: Data has been updated successfully.";
                        }
                    }

                }
                else
                {
                    db.DailyTimeSheets.Add(model);
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
            var report = db.DailyTimeSheets.Where(s => s.Id == id).FirstOrDefault();
            if (report != null)
            {
                fileName = report.FileName;
            }
            return fileName;
        }

        public string FetchFile(int id,string type)
        {
            string fileName = "";
            if (type == "DailyTimeSheet")
            {
                var report = db.DailyTimeSheets.Where(s => s.Id == id).FirstOrDefault();
                if (report != null)
                {
                    fileName = report.FileName;
                }
            }
            else if (type == "CallReport") {
                var report = db.CallOutReports.Where(s => s.Id == id).FirstOrDefault();
                if (report != null)
                {
                    fileName = report.FileName;
                }
            }
            else if (type == "PressureWashing")
            {
                var report = db.PressureWashingViewModels.Where(s => s.Id == id).FirstOrDefault();
                if (report != null)
                {
                    fileName = report.Filename;
                }
            }

            else if (type == "Template5")
            {
                var report = db.Template5.Where(s => s.Id == id).FirstOrDefault();
                if (report != null)
                {
                    fileName = report.FileName;
                }
            }

            else if (type == "CustomerTemplate")
            {
                var report = db.CustomerTempleteModels.Where(s => s.Id == id).FirstOrDefault();
                if (report != null)
                {
                    fileName = report.Filename;
                }
            }

            else if (type == "BusinessQuote")
            {
                var report = db.BussinessQuotesViewModels.Where(s => s.Id == id).FirstOrDefault();
                if (report != null)
                {
                    fileName = report.Filename;
                }
            }

            return fileName;
        }

    }
}