using GraphAlgorithms.Repository.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GraphAlgorithms.Web.Controllers
{
    
    public class LoginController : Controller
    {
        private readonly SignInManager<UserEntity> _signInManager;

        public LoginController(SignInManager<UserEntity> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl; // Store the ReturnUrl in ViewData
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string username, string password, string returnUrl = null)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, false, false);

            if (result.Succeeded)
            {
                // Redirect to the ReturnUrl if it's valid, otherwise go to Home
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid username or password.";
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        [Authorize]
        public string GetUserID()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }

}
