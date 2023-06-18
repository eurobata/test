using GoCardless;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TemplateManagementSystem.DAL;

namespace TemplateManagementSystem.BLL
{
    public class SubcriptionLogic
    {

        TemplateManagementSystemEntities db = new TemplateManagementSystemEntities();

        public string SaveUpdate(DAL.SubscriptionModel model)
        {
            string message = "";
            try
            {
                db.SubscriptionModels.Add(model);
                db.SaveChanges();


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

        public SubscriptionModel subscription (string id)
        {

            SubscriptionModel subscription = new SubscriptionModel();
            subscription=db.SubscriptionModels.Where(x=>x.AppUserId==id).FirstOrDefault();
            return subscription;

        }
        public bool delete(string id)
        {

            SubscriptionModel subscription = new SubscriptionModel();
            subscription = db.SubscriptionModels.Where(x => x.AppUserId == id).FirstOrDefault();
            db.SubscriptionModels.Remove(subscription);
            db.SaveChanges();
            return true;

        }

        public SubscriptionModel subscriptionBySub(string id)
        {

            SubscriptionModel subscription = new SubscriptionModel();
            subscription = db.SubscriptionModels.Where(x => x.PlanId == id).FirstOrDefault();
            return subscription;

        }

 
        public async Task <string> cancelsub(string subid)
        {
            SubscriptionModel subscription = new SubscriptionModel();
            subscription = db.SubscriptionModels.Where(x => x.PlanId == subid).FirstOrDefault();
            if(subscription != null)
            {
                String accessToken = ConfigurationManager.AppSettings["accesskey"];

                GoCardlessClient gocardless = GoCardlessClient.Create(accessToken, GoCardlessClient.Environment.LIVE);


                var subscriptionResponse = await gocardless.Subscriptions.CancelAsync(subid);

                subscription.status = "Cancelled";
                db.SaveChanges();
            }


            

            return "";


        }

        public async Task<string> Pausesub(string subid)
        {
            SubscriptionModel subscription = new SubscriptionModel();
            subscription = db.SubscriptionModels.Where(x => x.PlanId == subid).FirstOrDefault();
            if (subscription != null)
            {
                String accessToken = ConfigurationManager.AppSettings["accesskey"];

                
                GoCardlessClient gocardless = GoCardlessClient.Create(accessToken,GoCardlessClient.Environment.LIVE);

                var subscriptionResponse = await gocardless.Subscriptions.PauseAsync(subid);
                

              subscription.status = "Pause";
                db.SaveChanges();
            }




            return "";


        }
        public async Task<string> Resumesub(string subid)
        {
            SubscriptionModel subscription = new SubscriptionModel();
            subscription = db.SubscriptionModels.Where(x => x.PlanId == subid).FirstOrDefault();
            if (subscription != null)
            {
                String accessToken = ConfigurationManager.AppSettings["accesskey"];


                GoCardlessClient gocardless = GoCardlessClient.Create(accessToken, GoCardlessClient.Environment.LIVE);

                var subscriptionResponse = await gocardless.Subscriptions.ResumeAsync(subid);


                subscription.status = "Active";
                db.SaveChanges();
            }




            return "";


        }

    }
}