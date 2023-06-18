using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TemplateManagementSystem.DAL;
using TemplateManagementSystem.Models;

namespace TemplateManagementSystem.BLL
{
    public class FavouriteLogic
    {
        TemplateManagementSystemEntities db = new TemplateManagementSystemEntities();

        public List<DAL.Favourite> FetchFavourties(string userId)
        {
            return db.Favourites.Where(s => s.UserId == userId).ToList();
        }
        public void AddToFavourite(string userId, string templateType, string add)
        {
            var prev = db.Favourites.Where(s => s.UserId == userId && s.TemplateType == templateType).FirstOrDefault();
            if (add == "Yes")
            {
                if (prev == null)
                {
                    DAL.Favourite fav = new DAL.Favourite();
                    fav.CreatedDate = DateTime.Now;
                    fav.TemplateType = templateType;
                    fav.UserId = userId;
                    db.Favourites.Add(fav);
                    db.SaveChanges();
                }
            }
            else
            {
                if (prev != null)
                {
                    db.Favourites.Remove(prev);
                    db.SaveChanges();
                }
            }
        }
    }
}