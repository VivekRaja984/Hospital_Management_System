using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class Login : Controller
    {
        public IActionResult Loginpage()
        {
            return View();
        }
        public IActionResult Signup()
        {
            return View();
        }
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
