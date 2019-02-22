using BL.DTO;
using BL.Infrastructure;
using BL.ServiceFactory;
using BL.Services.DbAbstraction;
using ErrorChat.Models;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ErrorChat.Controllers
{
    [Authorize]
    public class accountController : Controller
    {
        private IUserService _userService;

        public accountController(IServiceFactory serviceFactory)
        {
            _userService = serviceFactory.CreateUserService();
        }


        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }


        [AllowAnonymous]
        public ActionResult Registration()
        {
            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Registration(RegistrationModel model)
        {
            await SetInitialDataAsync();
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO
                {
                    Password = model.Password,
                    UserName = model.UserName,
                    Role = "user"
                };
                OperationDetails operationDetails = await _userService.Create(userDto);
                if (operationDetails.Successed)
                {
                    ClaimsIdentity claim = await _userService.Authenticate(userDto);
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    return View("Success");
                }
                   
                else
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return View(model);
        }


        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            await SetInitialDataAsync();
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO { UserName = model.UserName, Password = model.Password };
                ClaimsIdentity claim = await _userService.Authenticate(userDto);
                if (claim == null)
                {
                    ModelState.AddModelError("", "Неверный логин или пароль");
                }
                else
                {
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    return RedirectToLocal(returnUrl);
                }
            }
            return View(model);
        }

        
        public ActionResult Success()
        {
            return View();
        }


        [AllowAnonymous]
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("main", "media");
        }
        

        private async Task SetInitialDataAsync()
        {
            await _userService.SetInitialData(new UserDTO
            {
                UserName = "lesha",
                Password = "alex28012001",
                Role = "admin"
            }, new List<string> { "user", "admin","moderatorAdvertising"});
        }


        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("main", "media");
        }
    }
}

