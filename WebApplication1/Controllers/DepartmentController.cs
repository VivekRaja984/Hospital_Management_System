using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult DepartmentList()
        {
            string connectionstr = _configuration.GetConnectionString("DefaultConnection");
            SqlConnection conn = new SqlConnection(connectionstr);
            conn.Open();

            SqlCommand cmd = new SqlCommand("PR_Department_SelectAll", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            conn.Close();

            return View(dt);
        }

        public IActionResult AddDepartment(int? id)
        {
            DepartmentModel model = new DepartmentModel();

            if (id != null)
            {
                SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                conn.Open();
                SqlCommand cmd = new SqlCommand("PR_Department_SelectByPK", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DepartmentID", id);
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);

                foreach (DataRow row in table.Rows)
                {
                    model.DepartmentID = Convert.ToInt32(row["DepartmentID"]);
                    model.DepartmentName = row["DepartmentName"].ToString();
                    model.Description = row["Description"].ToString();
                }
                conn.Close();
            }

            return View("AddDepartment", model);
        }

        [HttpPost]
        public IActionResult SaveDepartment(DepartmentModel model)
        {
            if (ModelState.IsValid)
            {
                SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;

                if (model.DepartmentID == 0)
                {
                    cmd.CommandText = "PR_Department_Insert";
                    cmd.Parameters.AddWithValue("@DepartmentName", model.DepartmentName);
                    cmd.Parameters.AddWithValue("@Description", model.Description ?? "");
                    cmd.Parameters.AddWithValue("@IsActive", true);
                    cmd.Parameters.AddWithValue("@Modified", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UserID", 1);
                }
                else
                {
                    cmd.CommandText = "PR_Department_UpdateByPK";
                    cmd.Parameters.AddWithValue("@DepartmentID", model.DepartmentID);
                    cmd.Parameters.AddWithValue("@DepartmentName", model.DepartmentName);
                    cmd.Parameters.AddWithValue("@Description", model.Description ?? "");
                    cmd.Parameters.AddWithValue("@IsActive", true);
                    cmd.Parameters.AddWithValue("@Modified", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UserID", 1);
                }

                cmd.ExecuteNonQuery();
                conn.Close();

                return RedirectToAction("DepartmentList");
            }

            return View("AddDepartment", model);
        }

        public IActionResult DeleteDepartment(int id)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            conn.Open();
            SqlCommand cmd = new SqlCommand("PR_Department_DeleteByPK", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DepartmentID", id);
            cmd.ExecuteNonQuery();
            conn.Close();

            return RedirectToAction("DepartmentList");
        }
    }

}
