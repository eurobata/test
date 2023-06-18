using GoCardless;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TemplateManagementSystem.BLL;

namespace TemplateManagementSystem.Controllers
{
    public class SubscriptionsController : Controller
    {
        // GET: Subscriptions
        public ActionResult Index()

        {
            if (!isadmin())
            {
                return RedirectToAction("Index", "home");

            }

            String accessToken = ConfigurationManager.AppSettings["accesskey"];
            GoCardlessClient gocardless = GoCardlessClient.Create(accessToken, GoCardlessClient.Environment.LIVE);

            var subscriptionRequest = new GoCardless.Services.SubscriptionListRequest()
            {

            };

                SubcriptionDataView subcriptionDataView = new SubcriptionDataView();
                var subscriptionListResponse = gocardless.Subscriptions.All(subscriptionRequest);
                List<GoCardless.Resources.Subscription> subscriptionList = new List<GoCardless.Resources.Subscription>();
                foreach (GoCardless.Resources.Subscription subscription in subscriptionListResponse)
                {
                    subcriptionDataView.subscriptionList.Add(subscription);

                    DAL.SubscriptionModel ud = new BLL.SubcriptionLogic().subscriptionBySub(subscription.Id);
                    if (ud != null)
                    {
                        subcriptionDataView.dbdata.Add(ud);

                    }

                }
            


                return View(subcriptionDataView);
            
        }

        [HttpPost]
        public async Task<ActionResult> cancel(string id)
        {
            if (!isadmin())
            {
                return RedirectToAction("Index", "home");

            }
            SubcriptionLogic subcriptionLogic = new SubcriptionLogic();
            string s = await subcriptionLogic.cancelsub(id);

            return RedirectToAction("index");

        }

        [HttpPost]
        public async Task<ActionResult> Resume(string id)
        {
            if (!isadmin())
            {
                return RedirectToAction("Index", "home");

            }

            SubcriptionLogic subcriptionLogic = new SubcriptionLogic();
            string s = await subcriptionLogic.Resumesub(id);

            return RedirectToAction("index");

        }
        [HttpPost]
        public async Task<ActionResult> Pause(string id)
        {
            if (!isadmin())
            {
                return RedirectToAction("Index", "home");

            }

            SubcriptionLogic subcriptionLogic = new SubcriptionLogic();
            string s = await subcriptionLogic.Pausesub(id);

            return RedirectToAction("index");

        }
        public bool isadmin()
        
        {
            bool ok = false;
             string admin= User.Identity.Name;
            if (ConfigurationManager.AppSettings["adminId"] ==User.Identity.GetUserId())
            {

                ok = true;
            }
            return ok;

        }


    }
    public class SubcriptionDataView {
        public SubcriptionDataView()
        {
            dbdata = new List<DAL.SubscriptionModel>();
            subscriptionList = new List<GoCardless.Resources.Subscription>();
        }
     public   List<GoCardless.Resources.Subscription> subscriptionList { get; set; }
     public   List<DAL.SubscriptionModel> dbdata { get; set; }
    
    }

}