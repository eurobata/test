using GoCardless;
using GoCardless.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TemplateManagementSystem.BLL;
using TemplateManagementSystem.Models;

namespace TemplateManagementSystem.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        
        // GET: Profile
        ApplicationDbContext ApplicationDbContext = new ApplicationDbContext();

        public ActionResult Index()
        {

            return View();
        }
        public ActionResult ShowProfile()
        {
            var manger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var CurrentUser = manger.FindById(User.Identity.GetUserId());
            ViewBag.ProfileName = CurrentUser.Email;
            ViewBag.FirstName=CurrentUser.FirstName;
            ViewBag.LastName=CurrentUser.LastName;
            ViewBag.Email = CurrentUser.Email;
            ViewBag.Address = CurrentUser.Address;
            ViewBag.ProfilePicture=CurrentUser.ProfilePicture;
            ViewBag.Phone=CurrentUser.PhoneNumber;
            ViewBag.add1 = CurrentUser.streatAddress2;
            ViewBag.add2 = CurrentUser.streatAddress3;
            ViewBag.office = CurrentUser.officeNo;
            ViewBag.city = CurrentUser.city;
            ViewBag.post = CurrentUser.postcode;
           
           




            return View();
        }
        public ActionResult EditProfile()
        {
            
            var manger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manger.FindById(User.Identity.GetUserId());
            ViewBag.ProfileName=currentUser.Email;
            ViewBag.PhoneNumber=currentUser.PhoneNumber;
            ViewBag.FirstName = currentUser.FirstName;
            ViewBag.LastName = currentUser.LastName;
            ViewBag.Email = currentUser.Email;
            ViewBag.Address = currentUser.Address;
            ViewBag.add1 = currentUser.streatAddress2;
            ViewBag.add2 = currentUser.streatAddress3;
            ViewBag.office = currentUser.officeNo;
            ViewBag.city = currentUser.city;
            ViewBag.post = currentUser.postcode;
            ViewBag.country = currentUser.Country;
            
            //ViewBag.ProfilePicture = currentUser.ProfilePicture;
            ViewBag.Phone = currentUser.PhoneNumber;
            ViewBag.ProfilePicture = !string.IsNullOrEmpty(currentUser.ProfilePicture) ? "/pics/" + currentUser.ProfilePicture : "/assets/images/Avatar.png";
            ViewBag.TemplateCount = new Common().FetchTemplateCount(User.Identity.GetUserId());
            DAL.SubscriptionModel subscription = new SubcriptionLogic().subscription(User.Identity.GetUserId());



            List<DropDownListModel> planlist = new List<DropDownListModel>();
            planlist.Add(new DropDownListModel() { Id = "Enterprise Plan", Name = "Enterprise Plan" });
            planlist.Add(new DropDownListModel() { Id = "Elite Plan", Name = "Elite Plan" });
            planlist.Add(new DropDownListModel() { Id = "Premium Plan", Name = "Premium Plan" });
            planlist.Add(new DropDownListModel() { Id = "Growing Plan", Name = "Growing Plan" });
            planlist.Add(new DropDownListModel() { Id = "Essential Plan", Name = "Essential Plan" });
            if (ConfigurationManager.AppSettings["adminId"] != User.Identity.GetUserId())
            {
                string plns = subscription.PlanName.Split('-')[0];
                var current = planlist.Where(x => x.Name == plns.Trim()).FirstOrDefault();
                if (current != null)
                {
                    planlist.Remove(current);


                }
                ViewBag.plan = planlist;

            }
            ViewBag.adminid = ConfigurationManager.AppSettings["adminId"];
            ViewBag.userid =User.Identity.GetUserId();




            return View(subscription);
        }

        [HttpPost]
        public ActionResult update(string name,string lastname, string phone, string address, string address2, string address3,  string officeno, string city, HttpPostedFileBase postedFile)
        {
            var manger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            string currentUsers = User.Identity.GetUserId();
           var currentUser = ApplicationDbContext.Users.Find(currentUsers);
            currentUser.Address = address;
            currentUser.streatAddress2 = address2;
            currentUser.streatAddress3 = address3;
            currentUser.FirstName = name;
            currentUser.LastName = lastname;
            currentUser.PhoneNumber = phone;
            currentUser.city = city;
            currentUser.officeNo = officeno;


            if (postedFile != null)
            {

                string fileName = System.IO.Path.GetFileName(postedFile.FileName);
                string path = Path.Combine(Server.MapPath("~/pics"), Path.GetFileName(postedFile.FileName));

                //Save the Image File in Folder.
                postedFile.SaveAs(path);

                currentUser.ProfilePicture = fileName;

            }



            ApplicationDbContext.SaveChanges();
           

            


            return RedirectToAction("EditProfile");
          }

        [HttpPost]
        public async Task< ActionResult> cancelSubs(DAL.SubscriptionModel model)
        {
            SubcriptionLogic subcriptionLogic = new SubcriptionLogic();
            string s =await subcriptionLogic.cancelsub(model.PlanId);


            return RedirectToAction("EditProfile");
       
        }
        [HttpPost]
        public async Task<ActionResult> changePlan(string upgrade)
        {

            GoCardlessClient client = GoCardlessClient.Create(
// We recommend storing your access token in an
// configuration setting for security
ConfigurationManager.AppSettings["accesskey"],

// Change me to LIVE when you're ready to go live
GoCardlessClient.Environment.LIVE

);



            var redirectFlowResponse = await client.RedirectFlows.CreateAsync(new RedirectFlowCreateRequest()
            {

                Description = "Cider Barrels",
                SessionToken = "asdaqwe2234234",
                SuccessRedirectUrl =
ConfigurationManager.AppSettings["returnUrl2"],


                // Optionally, prefill customer details on the payment page
              
                Metadata = new Dictionary<string, string>()
                 {
                   {"salesforce_id", "ABCD1234"}
                 }


            });

            var redirectFlow = redirectFlowResponse.RedirectFlow;

            // Hold on to this ID - you'll need it when you
            // "confirm" the redirect flow later
            Console.WriteLine(redirectFlow.Id);
            ViewData["Redicrected"] = redirectFlow;

            Session["plan"] = upgrade;
            return Redirect(redirectFlow.RedirectUrl);
        }

        public async Task<ActionResult> subscription(string redirect_flow_id)
        {
            try
            {
                GoCardlessClient client = GoCardlessClient.Create(
// We recommend storing your access token in an
// configuration setting for security
ConfigurationManager.AppSettings["accesskey"],

 // Change me to LIVE when you're ready to go live
 GoCardlessClient.Environment.LIVE
);
                var redirectFlowResponses = await client.RedirectFlows
        .CompleteAsync(redirect_flow_id,
            new RedirectFlowCompleteRequest()
            {
                SessionToken = "asdaqwe2234234"
            }
        );

                var redirectFlows = redirectFlowResponses.RedirectFlow;

                // Store the mandate ID against the customer's database record so you can charge
                // them in future
                Console.WriteLine($"Mandate: {redirectFlows.Links.Mandate}");
                Console.WriteLine($"Customer: {redirectFlows.Links.Customer}");

                // Display a confirmation page to the customer, telling them their Direct Debit has been
                // set up. You could build your own, or use ours, which shows all the relevant
                // information and is translated into all the languages we support.
                Console.WriteLine($"Confirmation URL: {redirectFlows.ConfirmationUrl}");

                string plan = Session["plan"].ToString();
                Plans pln = Plans.SelectedPlan(plan);

                var subscriptionRequest = new GoCardless.Services.SubscriptionCreateRequest()
                {
                    Amount = pln.amount,
                    Currency = "GBP",
                    Name = pln.name,
                    Interval = 1,

                    DayOfMonth = DateTime.Now.Day,


                    IntervalUnit = GoCardless.Services.SubscriptionCreateRequest.SubscriptionIntervalUnit.Monthly,
                    Links = new GoCardless.Services.SubscriptionCreateRequest.SubscriptionLinks()
                    {
                        Mandate = redirectFlows.Links.Mandate,
                    }
                };
                var subscriptionResponse = await client.Subscriptions.CreateAsync(subscriptionRequest);





                





                GoCardless.Resources.Subscription subscription = subscriptionResponse.Subscription;

                if (!string.IsNullOrEmpty(subscription.Id))
                {
                    bool si = new SubcriptionLogic().delete(User.Identity.GetUserId());


                    DateTime dateTime = Convert.ToDateTime(subscription.StartDate);

                    string sd = User.Identity.Name;
                    DAL.SubscriptionModel subscriptionModel = new DAL.SubscriptionModel();
                    subscriptionModel.AccountHoldername = User.Identity.Name;
                    subscriptionModel.AccountNo = "";
                    subscriptionModel.BranchCode = "";
                    subscriptionModel.subcriptionEnd = dateTime.AddMonths(1);
                    subscriptionModel.subcriptionStart = Convert.ToDateTime(subscription.StartDate);
                    subscriptionModel.GoCardlessId = redirectFlows.Links.Customer;
                    subscriptionModel.AppUserId = User.Identity.GetUserId();
                    subscriptionModel.CountryCode = "";
                    subscriptionModel.PlanName = pln.name + " -Monthly";
                    subscriptionModel.PlanId = subscription.Id;
                    subscriptionModel.status = subscription.Status;


                    string D = new SubcriptionLogic().SaveUpdate(subscriptionModel);



                    return RedirectToAction("Index", "Home");


                }
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ec)
            {
                return Content(ec.Message);
            }




        }

        public ActionResult FetchTemplateCount(string month, string year)
        {
            var count = new Common().FetchTemplateCount(User.Identity.GetUserId(), month, year);
            return Json(count, JsonRequestBehavior.AllowGet);
        }



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public JsonResult Update()
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user2 = UserManager.FindById(model.Id);
        //        RegisterViewModel model = new RegisterViewModel();
        //        var user = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        //        var currentuser= user.FindById(User.Identity.GetUserId());
        //        currentuser.FirstName = model.FirstName;
        //        currentuser.LastName = model.LastName;
        //        //currentuser.Email = model.Email;
        //        currentuser.Address = model.Address;
        //        currentuser.ProfilePicture = model.ProfilePicture;
        //        currentuser.Phone=model.Phone;

        //        UserManager.update(user);
        //    }
        //}


    }
}