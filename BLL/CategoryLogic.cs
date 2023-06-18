using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TemplateManagementSystem.DAL;
using TemplateManagementSystem.Models;

namespace TemplateManagementSystem.BLL
{
    public class CategoryLogic
    {
        TemplateManagementSystemEntities db = new TemplateManagementSystemEntities();

        public List<CategoryViewModel> GetAll()
        {
            var categories = db.Categories.ToList();
            List<string> lstUserIds = categories.Select(s => s.CreatedBy).ToList();
            var users = db.AspNetUsers.Where(s => lstUserIds.Contains(s.Id)).ToList();
            List<CategoryViewModel> lstCategories = new List<CategoryViewModel>();
            CategoryViewModel model;
            foreach (var item in categories)
            {
                model = new CategoryViewModel();
                model.Id = item.Id;
                model.CategoryName = item.CategoryName;
                model.CreatedDate = string.Format("{0:MM/dd/yyyy hh:mm tt}", item.CreatedDate);
                var user = users.Where(s => s.Id == item.CreatedBy).FirstOrDefault();
                if (user != null)
                {
                    model.CreatedBy = string.Format("{0} {1}", user.FirstName, user.LastName);
                }
                lstCategories.Add(model);
            }
            return lstCategories;
        }
        public CategoryViewModel FetchById(int id)
        {
            CategoryViewModel model = new CategoryViewModel();
            DAL.Category category = db.Categories.Where(s => s.Id == id).FirstOrDefault();
            if (category != null)
            {
                model.Id = category.Id;
                model.CategoryName = category.CategoryName;
                model.CreatedDate = string.Format("{0:MM/dd/yyyy hh:mm tt}", category.CreatedDate);
            }
            return model;
        }

        public TemplateCategoryViewModel FetchTemplatesByCategoryId(int id)
        {
            TemplateCategoryViewModel model = new TemplateCategoryViewModel();
            DAL.Category category = db.Categories.Where(s => s.Id == id).FirstOrDefault();
            if (category != null)
            {
                model.Id = category.Id;
                model.CategoryId = category.Id;
                model.TemplateIds = category.TemplateCategories.Select(s => s.TemplateId).ToList();
            }
            return model;
        }

        public string SaveUpdate(DAL.Category model)
        {
            string message = "";
            try
            {
                if (model.Id > 0)
                {
                    Category oldCategory = db.Categories.Where(s => s.Id == model.Id).FirstOrDefault();
                    if (oldCategory != null)
                    {
                        oldCategory.CategoryName = model.CategoryName;

                        db.SaveChanges();
                        message = "Success: Data has been updated successfully.";
                    }
                }
                else
                {
                    db.Categories.Add(model);
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

        public string SaveUpdateTemplates(List<TemplateCategory> model)
        {
            string message = "";
            try
            {
                if (model.Count > 0)
                {
                    int? categoryId = model.FirstOrDefault().CategoryId;
                    var oldTemplates = db.TemplateCategories.Where(s => s.CategoryId == categoryId).ToList();
                    db.TemplateCategories.RemoveRange(oldTemplates);
                    db.TemplateCategories.AddRange(model);
                    if (oldTemplates.Count > 0)
                    {
                        message = "Success: Data has been updated successfully.";
                    }
                    else
                    {
                        message = "Success: Data has been saved successfully.";
                    }

                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                message = "Error: " + ex.Message;
            }
            return message;
        }

        public List<DropDownListModel> GetCategories()
        {
            return db.Categories.Select(s => new DropDownListModel() { Id = s.Id.ToString(), Name = s.CategoryName }).ToList();
        }

        public List<TemplateCategory> GetTemplateCategories()
        {
            return db.TemplateCategories.ToList();
        }
        
    }
}