using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GoCardless;
using GoCardless.Errors;
using GoCardless.Resources;
using GoCardless.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using TemplateManagementSystem.BLL;
using TemplateManagementSystem.Models;

namespace TemplateManagementSystem.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        private TempletDbContext _TempletDbContext;
        public AccountController()
        {
        
        }

        // LIVE_ywFNKMDv7aQ2EPvndX3gYbn9y2Ef7e2UzOuFvWD_


        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager,TempletDbContext templetDb )
        {

            _TempletDbContext = templetDb;
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Signin()
        {
            ViewBag.ReturnUrl = "";
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Signin(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var user = await UserManager.FindByNameAsync(model.Email);
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    {
                        Session["ProfilePicture"] = !string.IsNullOrEmpty(user.ProfilePicture) ? "/Uploads/"+ user.ProfilePicture : "/assets/images/Avatar.png";
                        return RedirectToAction("panel", "home");

                    }

                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToAction("panel","home");

                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Signup()
        {
            ViewBag.Countries = new Common().FetchCountries();
            List<DropDownListModel> planlist = new List<DropDownListModel>();
            planlist.Add(new DropDownListModel() { Id= "Gold Plan", Name= "Gold Plan - £129.99" });
            planlist.Add(new DropDownListModel() { Id= "Elite Plan" ,Name= "Elite Plan - £49.99" });
            planlist.Add(new DropDownListModel() { Id= "Premium Plan" ,Name= "Premium Plan - £19.99" });
            planlist.Add(new DropDownListModel() { Id= "Growing Plan" ,Name= "Growing Plan - £9.99" });
            planlist.Add(new DropDownListModel() { Id= "Essential Plan" ,Name= "Essential Plan - £3.99" });

            ViewBag.plans=planlist;
            
            return View();
        }
        [AllowAnonymous]

        public ActionResult termsConditions()
        {
            return View();
        }
        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Signup(RegisterViewModel model)
        {
            if (ModelState.IsValid)
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


                    //var customerRequest = new GoCardless.Services.CustomerCreateRequest()
                    //{
                    //    Email = model.Email,
                    //    GivenName = model.FirstName+" "+ model.LastName,
                    //    FamilyName = model.FirstName,
                    //    AddressLine1 = model.Address,
                    //    AddressLine2 = model.Address2,
                    //    City =model.city,
                    //    PostalCode = model.PostCode,
                    //    CountryCode = model.CountryCode,

                    //};


                //    var customerRequest = new GoCardless.Services.CustomerCreateRequest()
                //    {
                //        Email = model.Email,
                //        GivenName = model.FirstName + " " + model.LastName,
                //        FamilyName = model.FirstName,
                //        AddressLine1 = model.Address,
                //        AddressLine2 = model.Address2,
                //        City = model.city,
                //        PostalCode = model.PostCode,
                //        CountryCode = model.CountryCode,

                //        Metadata = new Dictionary<string, string>()
                //{
                //  {"salesforce_id", "ABCD1234"}
                //}
                //    };
                //    var customerResponse = await client.Customers.CreateAsync(customerRequest);
                //    GoCardless.Resources.Customer customer = customerResponse.Customer;



                //    var customerBankAccountRequest = new GoCardless.Services.CustomerBankAccountCreateRequest()
                //    {
                //        AccountHolderName = model.AccountHoldername,
                //        AccountNumber = model.AccountNo,
                //        BranchCode = model.BranchCode,
                //        CountryCode = model.CountryCode,
                //        Links = new GoCardless.Services.CustomerBankAccountCreateRequest.CustomerBankAccountLinks()
                //        {
                //            Customer = customer.Id
                //        }
                //    };

                //    var customerBankAccountResponse = await client.CustomerBankAccounts.CreateAsync(customerBankAccountRequest);
                //    GoCardless.Resources.CustomerBankAccount customerBankAccount = customerBankAccountResponse.CustomerBankAccount;
                //    var mandateRequest = new GoCardless.Services.MandateCreateRequest()
                //    {
                //        Links = new GoCardless.Services.MandateCreateRequest.MandateLinks()
                //        {
                //            CustomerBankAccount = customerBankAccount.Id
                //        },
                ////        Metadata = new Dictionary<string, string>()
                ////{
                ////    {"internal_reference", "ref_09011991"}
                ////},
                //        Scheme = "bacs"
                //    };

                //    var mandateResponse = await client.Mandates.CreateAsync(mandateRequest);
                //    GoCardless.Resources.Mandate mandate = mandateResponse.Mandate;

                    string country = model.Country.Split('-')[0];
                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, PhoneNumber = model.Phone, Address = model.Address, Agree = model.Agree, Country = country, CreatedDate = DateTime.Now, streatAddress2 = model.Address2, streatAddress3 = model.Address3, postcode = model.PostCode, city = model.city, officeNo = model.OfficeNo };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                    
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                        // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                        // Send an email with this link
                        // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                        var redirectFlowResponse = await client.RedirectFlows.CreateAsync(new RedirectFlowCreateRequest()
                        {

                            Description = "Cider Barrels",
                            SessionToken = "asdaqwe2234234",
                            SuccessRedirectUrl =
 ConfigurationManager.AppSettings["returnUrl"],
                          

                            // Optionally, prefill customer details on the payment page
                            PrefilledCustomer = new RedirectFlowCreateRequest.RedirectFlowPrefilledCustomer()
                            {
                                GivenName = user.FirstName+" "+user.LastName,
                                FamilyName = user.UserName,
                                Email =user.Email,
                                AddressLine1 = user.Address,
                                City = user.city,
                                PostalCode = user.postcode,
                                


                            },
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
                        
                        Session["plan"] = model.plans;
                        return Redirect(redirectFlow.RedirectUrl);




                        return RedirectToAction("Index", "Home");



                    }
                    AddErrors(result);




                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                
           
            }

            ViewBag.Countries = new Common().FetchCountries();
            List<DropDownListModel> planlist = new List<DropDownListModel>();
            planlist.Add(new DropDownListModel() { Id = "Gold Plan", Name = "Gold Plan - £129.99" });
            planlist.Add(new DropDownListModel() { Id = "Elite Plan", Name = "Elite Plan - £49.99" });
            planlist.Add(new DropDownListModel() { Id = "Premium Plan", Name = "Premium Plan - £19.99" });
            planlist.Add(new DropDownListModel() { Id = "Growing Plan", Name = "Growing Plan - £9.99" });
            planlist.Add(new DropDownListModel() { Id = "Essential Plan", Name = "Essential Plan - £3.99" });

            ViewBag.plans = planlist;

            // If we got this far, something failed, redisplay form
            return View(model);
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

                    DateTime dateTime = Convert.ToDateTime(subscription.StartDate);

                    string sd = User.Identity.Name;
                    DAL.SubscriptionModel subscriptionModel = new DAL.SubscriptionModel();
                    subscriptionModel.AccountHoldername =User.Identity.Name;
                    subscriptionModel.AccountNo = "";
                    subscriptionModel.BranchCode = "";
                    subscriptionModel.subcriptionEnd = dateTime.AddMonths(1);
                    subscriptionModel.subcriptionStart = Convert.ToDateTime(subscription.StartDate);
                    subscriptionModel.GoCardlessId = redirectFlows.Links.Customer;
                    subscriptionModel.AppUserId = User.Identity.GetUserId();
                    subscriptionModel.CountryCode = "";
                    subscriptionModel.PlanName = pln.name+" -Monthly";
                    subscriptionModel.PlanId = subscription.Id;
                    subscriptionModel.status = subscription.Status;


                    string D = new SubcriptionLogic().SaveUpdate(subscriptionModel);



                    return RedirectToAction("panel", "home");



                }
                return RedirectToAction("panel", "home");
            }
            catch (Exception ec)
            {
                return Content(ec.Message);
            }
           



        }
        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                 string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                 var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                 
                //await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
             
                 BLL.Common common=new BLL.Common();
             bool abc=   common.SendEmail(model.Email, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>", null, null) ;


                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("panel", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}