using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class Login : Controller
    {
        private readonly IConfiguration _configuration;

        public Login(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Loginpage()
        {
            return View();
        }
        
        public IActionResult Signup()
        {
            return View(new User());
        }

        [HttpPost]
        public IActionResult Signup(User model)
        {
            if (ModelState.IsValid)
            {
                string connectionStr = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection conn = new SqlConnection(connectionStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(); 
                    cmd.Connection= conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "PR_User_Insert";
                    cmd.Parameters.AddWithValue("@Username", model.Username);
                    cmd.Parameters.AddWithValue("@Password", model.Password);
                    cmd.Parameters.AddWithValue("@Email", model.Email);
                    cmd.Parameters.AddWithValue("@MobileNo", model.MobileNo);
                    cmd.Parameters.AddWithValue("@IsActive",true);
                    cmd.Parameters.AddWithValue("@Modified", DateTime.Now);

                    cmd.ExecuteNonQuery();
                }

                
                return RedirectToAction("Loginpage");
            }
            return View("Signup", model);
        }
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
