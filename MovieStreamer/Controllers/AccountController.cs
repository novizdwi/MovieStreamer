using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MovieStreamer.ViewModels;
using MovieStreamer.Models;
using MovieStreamer.Services;
using PagedList;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace MovieStreamer.Controllers
{
    public class AccountController : Controller
    {
        protected IHttpContextAccessor contextAccessor;
        protected ApplicationDbContext context;
        private AccountService services;
        public AccountController(IHttpContextAccessor _contextAccessor,
            ApplicationDbContext _context,
            AccountService services)
        {
            contextAccessor = _contextAccessor;
            context = _context;
            this.services = services;
        }


        public IActionResult Index()
        {
            return LocalRedirect("/");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            string msg = "";
            var success = true;
            if (ModelState.IsValid)
            {
                if (String.IsNullOrEmpty(viewModel.Email) && String.IsNullOrEmpty(viewModel.Phone))
                {
                    msg = "Email or Phone must not null";
                    success = false;
                }
                if (viewModel.Password != viewModel.ConfirmPassword)
                {
                    msg = msg.Length!=0? msg + "<br/> Password input is not same": "Password input is not same";
                    success = false;
                }
                if (success)
                {
                    var isExist = services.CekExist(viewModel);
                    if (isExist !=0)
                    {
                        msg = "Email or Phone already exist";
                    }
                    else 
                    { 
                        var result = await services.Register(viewModel);
                        if (result.Succeeded)
                        {
                            success = true;
                            var claims = new List<Claim>() {
                                new Claim(ClaimTypes.NameIdentifier, result.Id),
                                new Claim(ClaimTypes.Email, result.Email),
                                new Claim(ClaimTypes.MobilePhone, result.Phone),
                            };
                            //Initialize a new instance of the ClaimsIdentity with the claims and authentication scheme
                            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            //Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity
                            var principal = new ClaimsPrincipal(identity);
                        }
                        else
                        {
                            msg = "An Error Occured";
                        }
                    }
                }
            }
            ViewBag.Message = msg;
            return View(viewModel);
        }

        public IActionResult Login(string ReturnUrl = "/")
        {
            LoginViewModel viewModel = new LoginViewModel();
            viewModel.ReturnUrl = ReturnUrl;
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid) {
                var result = services.Login(viewModel);
                if(result == null)
                {
                    ViewBag.Message = "Invalid Credential";
                }
                else
                {
                    var claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier,Convert.ToString(result.Id)),
                                new Claim(ClaimTypes.NameIdentifier, Convert.ToString(result.Id)),
                                new Claim(ClaimTypes.Email, result.Email),
                                new Claim(ClaimTypes.MobilePhone, result.Phone),
                    };
                    //Initialize a new instance of the ClaimsIdentity with the claims and authentication scheme
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    //Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity
                    var principal = new ClaimsPrincipal(identity);
                    //SignInAsync is a Extension method for Sign in a principal for the specified scheme.
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        principal, new AuthenticationProperties() { IsPersistent = viewModel.RememberLogin });

                    return LocalRedirect(viewModel.ReturnUrl);
                }
            }
            return View(viewModel);
        }
        public async Task<IActionResult> LogOut()
        {
            //SignOutAsync is Extension method for SignOut
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //Redirect to home page
            return LocalRedirect("/");
        }
    }
}
