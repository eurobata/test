using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TemplateManagementSystem.DAL;
using TemplateManagementSystem.Models;

namespace TemplateManagementSystem.BLL
{
    public class CallOutReportLogic
    {
        TemplateManagementSystemEntities db = new TemplateManagementSystemEntities();
        public CallOutReportViewModel FetchById(int id)
        {
            CallOutReportViewModel model = new CallOutReportViewModel();
            var callOutReport = db.CallOutReports.Where(s => s.Id == id).FirstOrDefault();
            if (callOutReport != null)
            {
                model.Id = callOutReport.Id;
                model.ClientName = callOutReport.ClientName;
                model.ArrivalTime = callOutReport.ArrivalTime;
                model.JobAddress = callOutReport.JobAddress;
                model.DetailOfWork = callOutReport.DetailOfWork;
                model.Date = callOutReport.Date;
                model.DateStr = string.Format("{0:yyyy-MM-dd}", callOutReport.Date);

                model.DepartTime = callOutReport.DepartTime;
                model.ContactTelephone = callOutReport.ContactTelephone;
                model.EngineerName = callOutReport.EngineerName;
                model.JobNo = callOutReport.JobNo;
            }
            return model;
        }

        public string SaveUpdate(CallOutReport model,string Method)
        {
            string message = "";
            try
            {
                if (model.Id > 0)
                {
                    if (Method == "Copy")
                    {
                        db.CallOutReports.Add(model);
                        db.SaveChanges();
                        message = "Success: Data has been saved successfully.";

                    }
                    else
                    {
                        CallOutReport oldReport = db.CallOutReports.Where(s => s.Id == model.Id).FirstOrDefault();
                        if (oldReport != null)
                        {
                            oldReport.ClientName = model.ClientName;
                            oldReport.JobAddress = model.JobAddress;
                            oldReport.Date = model.Date;
                            oldReport.ModifiedDate = model.ModifiedDate;
                            oldReport.ModifiedBy = model.ModifiedBy;
                            oldReport.ContactTelephone = model.ContactTelephone;
                            oldReport.EngineerName = model.EngineerName;
                            oldReport.DetailOfWork = model.DetailOfWork;
                            oldReport.ArrivalTime = model.ArrivalTime;
                            oldReport.DepartTime = model.DepartTime;
                            oldReport.FileName = model.FileName;
                            oldReport.JobNo = model.JobNo;

                            db.SaveChanges();
                            message = "Success: Data has been updated successfully.";
                        }

                    }
                   
                }
                else
                {
                    db.CallOutReports.Add(model);
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
            var report = db.CallOutReports.Where(s => s.Id == id).FirstOrDefault();
            if (report != null)
            {
                fileName = report.FileName;
            }
            return fileName;
        }
    }
}