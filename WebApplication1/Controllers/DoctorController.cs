using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IConfiguration _configuration;


        public DoctorController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult DoctorList()
        {
            string connectionstr = _configuration.GetConnectionString("DefaultConnection");

            SqlConnection conn = new SqlConnection(connectionstr);
            conn.Open();

            SqlCommand objCmd = conn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_Doctor_SelectAll";

            SqlDataReader objSDR = objCmd.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(objSDR);

            conn.Close();

            return View(dt);
        }
        
        public IActionResult AddDoctor(int? DoctorID)
        {
            Doctor model = new Doctor();

            if (DoctorID != null)
            {
                string connectionstr = _configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection conn = new SqlConnection(connectionstr))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand("PR_Doctor_SelectByPK", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@DoctorID", DoctorID);

                    SqlDataReader reader = command.ExecuteReader();
                    DataTable table = new DataTable();
                    table.Load(reader);

                    foreach (DataRow row in table.Rows)
                    {
                        model.DoctorID = Convert.ToInt32(row["DoctorID"]);
                        model.Name = row["Name"].ToString();
                        model.Phone = row["Phone"].ToString();
                        model.Email = row["Email"].ToString();
                        model.Qualification = row["Qualification"].ToString();
                        model.Specialization = row["Specialization"].ToString();
                    }
                }
            }

            return View(model);
        }

        public IActionResult Delete(int DoctorID)
        {
            string connectionstr = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection conn = new SqlConnection(connectionstr))
            {
                conn.Open();
                using (SqlCommand sqlCommand = conn.CreateCommand())
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "PR_Doctor_DeleteByPK";
                    sqlCommand.Parameters.AddWithValue("@DoctorID", DoctorID);
                    sqlCommand.ExecuteNonQuery();
                }
            }
            return RedirectToAction("DoctorList");
        }
        [HttpPost]
        public IActionResult SaveDoctor(Doctor model)
        {
            if (ModelState.IsValid)
            {
                string connectionstr = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection conn = new SqlConnection(connectionstr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (model.DoctorID == 0)
                    {
                        cmd.CommandText = "PR_Doctor_Insert";
                        cmd.Parameters.AddWithValue("@Name", model.Name);
                        cmd.Parameters.AddWithValue("@Phone", model.Phone);
                        cmd.Parameters.AddWithValue("@Email", model.Email);
                        cmd.Parameters.AddWithValue("@Qualification", model.Qualification);
                        cmd.Parameters.AddWithValue("@Specialization", model.Specialization);
                        cmd.Parameters.AddWithValue("@IsActive", true);
                        cmd.Parameters.AddWithValue("@Modified", DateTime.Now);
                        cmd.Parameters.AddWithValue("@UserID", 1); 
                    }
                    else
                    {
                        cmd.CommandText = "PR_Doctor_UpdateByPK";
                        cmd.Parameters.AddWithValue("@DoctorID", model.DoctorID);
                        cmd.Parameters.AddWithValue("@Name", model.Name);
                        cmd.Parameters.AddWithValue("@Phone", model.Phone);
                        cmd.Parameters.AddWithValue("@Email", model.Email);
                        cmd.Parameters.AddWithValue("@Qualification", model.Qualification);
                        cmd.Parameters.AddWithValue("@Specialization", model.Specialization);
                        cmd.Parameters.AddWithValue("@IsActive", true);
                        cmd.Parameters.AddWithValue("@Modified", DateTime.Now);
                        cmd.Parameters.AddWithValue("@UserID", 1); 
                    }

                    cmd.ExecuteNonQuery();
                }

                return RedirectToAction("DoctorList");
            }

            return View("AddDoctor", model);
        }


    }
}
