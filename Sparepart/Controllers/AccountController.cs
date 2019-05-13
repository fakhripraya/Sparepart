using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Sparepart.Models;

namespace Sparepart.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        ApplicationDbContext Context;
        dbsparepartEntities Dbcontext = new dbsparepartEntities(); 
        public AccountController()
        {
            Context = new ApplicationDbContext();
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
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

        //GET: /Account/Index
        [Authorize(Roles = "MasterUser")]
        public ActionResult Index()
        {
            var role = Dbcontext.masterroles.ToList();
            ViewData["Role"] = role;
            var cabang = Dbcontext.mastercabangs.ToList();
            ViewData["Cabang"] = cabang;
            return View();
        }
        public JsonResult GetProducts(string text)
        {
            var northwind = new dbsparepartEntities();

            var products = northwind.masterroles.Where(x => x.IsDelete == 0).Select(product => new RoleViewModel
            {
                RoleID = product.RoleID,
                NamaRole = product.NamaRole
            });

            if (!string.IsNullOrEmpty(text))
            {
                products = products.Where(p => p.NamaRole.Contains(text));
            }

            return Json(products, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "MasterUser")]
        public ActionResult Users_Read([DataSourceRequest]DataSourceRequest request)
        {

            DataSourceResult result = Dbcontext.masterusers.Where(x => x.IsDelete == 0).ToDataSourceResult(request,
                model => new masteruser
                {
                    UserID = model.UserID,
                    NamaUser = model.NamaUser,
                    Username = model.Username,
                    RoleID = model.RoleID,
                    CabangID = model.CabangID,
                    Email = model.Email,
                    IsDelete = model.IsDelete
                });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "MasterUser")]
        public ActionResult Users_Destroy([DataSourceRequest]DataSourceRequest request, masteruser user)
        {

            if(ModelState.IsValid)
            {
                using (var db = new dbsparepartEntities())
                {
                    var entity = db.masterusers.Where(u => u.UserID == user.UserID).FirstOrDefault();
                    entity.IsDelete = 1;
                    db.SaveChanges();
                    ApplicationUser aspuser = Context.Users.Find(user.UserID);
                    Context.Users.Remove(aspuser);
                    Context.SaveChanges();
                }
            }
            return Json(new[] { user }.ToDataSourceResult(request, ModelState));
        }
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MasterUser")]
        public ActionResult Users_Update(UserUpdateViewModel model)
        {
            string id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            masteruser CurrentUser = Dbcontext.masterusers.Where(x => x.UserID == id).FirstOrDefault();
            int RoleID = Int32.Parse(Request.Form["role"]);
            int CabangID = Int32.Parse(Request.Form["cabang"]);
            if (ModelState.IsValid)
            {
                masteruser olduser = Dbcontext.masterusers.AsNoTracking().Where(x => x.UserID == model.UserID).FirstOrDefault();
                ApplicationUser aspuser = UserManager.FindByIdAsync(model.UserID).Result;
                aspuser.UserName = model.UserName;
                aspuser.Email = model.Email;
                UserManager.Update(aspuser);
                //UserManager.RemovePasswordAsync(aspuser.Id);
                //UserManager.AddPasswordAsync(aspuser.Id, model.Password);

                foreach (var value in Dbcontext.akses.Where(x => x.RoleID == olduser.RoleID))
                {
                    UserManager.RemoveFromRoleAsync(aspuser.Id, value.NamaAkses);
                }

                foreach (var value in Dbcontext.akses.Where(x => x.RoleID == RoleID))
                {
                    UserManager.AddToRoleAsync(aspuser.Id, value.NamaAkses);
                }

                using (var northwind = new dbsparepartEntities())
                {
                    var entity = northwind.masterusers.Where(u => u.UserID == model.UserID).FirstOrDefault();
                    entity.NamaUser = model.NamaUser;
                    entity.Username = model.UserName;
                    entity.Email = model.Email;
                    entity.RoleID = RoleID;
                    entity.CabangID = CabangID;
                    entity.UserUpdate = CurrentUser.NamaUser;
                    entity.TanggalUpdate = DateTime.Now;
                    northwind.SaveChanges();
                }
                return RedirectToAction("Index", "Account");
            }
            return RedirectToAction("Index", "Account");
        }

        // POST: /Account/Register
        [Authorize(Roles = "MasterUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public /*async Task<ActionResult>*/ ActionResult Register(RegisterViewModel model)
        {
            model.Password = "123456";
            var sel = Dbcontext.masterusers.Where(x => x.Username == model.UserName).FirstOrDefault();
            if (sel != null && sel.IsDelete == 0)
            {
                return Json(new { success = true, responseText = "User" + " " +  model.UserName + " " + "Already been registered!" }, JsonRequestBehavior.AllowGet);
            }
            else if (ModelState.IsValid)
            {
                string id = System.Web.HttpContext.Current.User.Identity.GetUserId();
                masteruser CurrentUser = Dbcontext.masterusers.Where(x => x.UserID == id).FirstOrDefault();
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                var result = UserManager.CreateAsync(user, model.Password);
                //if (result.Succeeded)
                //{
                    int RoleID = Int32.Parse(model.UserRoles);
                    int CabangID = Int32.Parse(model.UserCabang);
                    foreach (var value in Dbcontext.akses.Where(x => x.RoleID == RoleID))
                    {
                        this.UserManager.AddToRoleAsync(user.Id, value.NamaAkses);
                    }

                    //Ends Here 
                    masteruser u = new masteruser();
                    u.UserID = user.Id;
                    u.NamaUser = model.NamaUser;
                    u.Username = model.UserName;
                    u.Email = model.Email;
                    u.Password = user.PasswordHash;
                    u.RoleID = RoleID;
                    u.CabangID = CabangID;
                    u.UserInput = CurrentUser.NamaUser;
                    u.TanggalInput = System.DateTime.Now;
                    u.IsDelete = 0;
                    Dbcontext.masterusers.Add(u);
                    Dbcontext.SaveChanges();
                return Json(new { success = true, responseText = "Sucessfully create a user" }, JsonRequestBehavior.AllowGet);
                //}
                //AddErrors(result);
            }
            // If we got this far, something failed, redisplay form
            return RedirectToAction("Index");
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (!User.Identity.IsAuthenticated)
            {
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
            else
            {
                return RedirectToLocal(returnUrl);
            }
            
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model ,string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
                {
                    case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
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
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
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
                masteruser CurrentUser = Dbcontext.masterusers.Where(x => x.Email == model.Email).FirstOrDefault();
                var user = await UserManager.FindByEmailAsync(model.Email);


                //if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                //{
                //    // Don't reveal that the user does not exist or is not confirmed
                //    return View("Login");
                //}

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771


                //Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress(model.Email));  // replace with valid value 
                message.From = new MailAddress("PSPTWebSystem@indomaret.co.id");  // replace with valid value
                message.Subject = "Confirmation Email";
                message.Body = string.Format(body, "PSPT WEB System", "PSPTWebSystem@indomaret.co.id", callbackUrl);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Host = "192.168.2.240";
                    smtp.Port = 25;
                    //smtp.EnableSsl = true;
                    smtp.Send(message);
                    return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }


                //await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                //return RedirectToAction("ForgotPasswordConfirmation", "Account");
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
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