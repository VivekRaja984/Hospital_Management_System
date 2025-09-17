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
        [HttpGet]
        public IActionResult Loginpage()
        {
            return View(new LoginViewModel());
        }
        
        [HttpPost]
        public IActionResult Loginpage(LoginViewModel user)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    string connectionString = this._configuration.GetConnectionString("DefaultConnection");
                    SqlConnection conn = new SqlConnection(connectionString);
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType= CommandType.StoredProcedure;
                    cmd.CommandText = "PR_User_Login";
                    cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = user.Email;
                    cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = user.Password;
                    SqlDataReader sqlDataReader =cmd.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(sqlDataReader);
                    if (dataTable.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dataTable.Rows)
                        {
                            HttpContext.Session.SetString("UserID", dr["UserId"].ToString());
                            HttpContext.Session.SetString("UserName", dr["Username"].ToString());
                            HttpContext.Session.SetString("EmailAddress", dr["Email"].ToString());
                        }

                        return RedirectToAction("Dashboard");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "User is not found";
                        return RedirectToAction("Loginpage");
                    }
                }
            }
            catch(Exception e)
            {
                TempData["ErrorMessage"]= e.Message;
            }
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
        [CheckAccess]
        public IActionResult Dashboard()
        {
            string connectionstr = _configuration.GetConnectionString("DefaultConnection");

            SqlConnection conn = new SqlConnection(connectionstr);
            conn.Open();

            SqlCommand objCmd = conn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_Appointment_SelectAll";

            SqlDataReader objSDR = objCmd.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(objSDR);

            conn.Close();
            int completedCount = dt.AsEnumerable().Count(row => row.Field<string>("AppointmentStatus") == "Completed");
            int pendingCount = dt.AsEnumerable().Count(row => row.Field<string>("AppointmentStatus") == "Pending");

            
            ViewBag.CompletedCount = completedCount;
            ViewBag.PendingCount = pendingCount;
            ViewBag.TotalAppointments = dt.Rows.Count;

            return View(dt);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); 
            return RedirectToAction("Loginpage", "Login");
        }

    }
}
