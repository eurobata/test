using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TemplateManagementSystem.DAL;
using TemplateManagementSystem.Models;

namespace TemplateManagementSystem.BLL
{
    public class TemplateHistoryLogic
    {
        TemplateManagementSystemEntities db = new TemplateManagementSystemEntities();
        public List<FetchTemplateHistory_Result> FetchHistory(DateTime startDate, string currentUser, DateTime endDate)
        {
            return db.FetchTemplateHistory(startDate, endDate).ToList();


        }

        public List<SelectListItem> GetTemplates()
        {
            return db.SystemTemplates.Select(s => new SelectListItem() { Value = s.Id.ToString(), Text = s.TemplateName }).ToList();
        }
    }
}