using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TemplateManagementSystem.BLL;
using TemplateManagementSystem.DAL;
using TemplateManagementSystem.Models;

namespace TemplateManagementSystem.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            return View(new CategoryLogic().GetAll());
        }

        public ActionResult AddEditCategory(int? id)
        {
            CategoryViewModel model = new CategoryViewModel();
            if (id.HasValue)
            {
                model = new CategoryLogic().FetchById(id.Value);
            }
            return PartialView("_AddEditCategory", model);
        }

        public ActionResult AddEditTemplateCategory(int? id)
        {
            TemplateCategoryViewModel model = new TemplateCategoryViewModel();
            if (id.HasValue)
            {
                model = new CategoryLogic().FetchTemplatesByCategoryId(id.Value);
                model.TemplateSelected = string.Join(",", model.TemplateIds);
            }
            List<SelectListItem> lstTemplates = new List<SelectListItem>();

            var categories = new CategoryLogic().GetAll();
            ViewBag.lstCategories = categories.Select(s => new DropDownListModel() { Id = s.Id.ToString(),Name = s.CategoryName}).ToList();

            model.Templates = new TemplateHistoryLogic().GetTemplates();
            
            return PartialView("_AddEditTemplate", model);
        }

        [HttpPost]
        public ActionResult AddEditTemplateCategory(TemplateCategoryViewModel model)
        {
            List<TemplateCategory> lstTemplates = new List<TemplateCategory>();
            DAL.TemplateCategory category;
            foreach (var item in model.TemplateIds)
            {
                category = new TemplateCategory();
                category.CategoryId = model.CategoryId;
                category.TemplateId = item;
                category.CreatedDate = DateTime.Now;
                category.CreatedBy = User.Identity.GetUserId();
                lstTemplates.Add(category);
            }

            new CategoryLogic().SaveUpdateTemplates(lstTemplates);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddEditCategory(CategoryViewModel model)
        {
            DAL.Category category = new DAL.Category();
            category.CategoryName = model.CategoryName;
            category.Id = model.Id;
            category.CreatedDate = DateTime.Now;
            category.CreatedBy = User.Identity.GetUserId();
            new CategoryLogic().SaveUpdate(category);
            return RedirectToAction("Index");
        }
    }
}