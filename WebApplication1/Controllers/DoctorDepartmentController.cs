using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class DoctorDepartmentController : Controller
    {
        private readonly IConfiguration _configuration;

        public DoctorDepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult DoctorDepartmentList()
        {
            string connectionstr = _configuration.GetConnectionString("DefaultConnection");
            SqlConnection conn = new SqlConnection(connectionstr);
            conn.Open();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_DoctorDepartment_SelectAll";

            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            conn.Close();

            return View(dt);
        }

        public IActionResult AddDoctorDepartment(int? DoctorDepartmentID)
        {
            DoctorDepartment model = new DoctorDepartment();

            if (DoctorDepartmentID != null)
            {
                string connStr = _configuration.GetConnectionString("DefaultConnection");
                using SqlConnection conn = new SqlConnection(connStr);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PR_DoctorDepartment_SelectByPK", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DoctorDepartmentID", DoctorDepartmentID);

                SqlDataReader reader = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                foreach (DataRow row in table.Rows)
                {
                    model.DoctorDepartmentID = Convert.ToInt32(row["DoctorDepartmentID"]);
                    model.DoctorID = Convert.ToInt32(row["DoctorID"]);
                    model.DepartmentID = Convert.ToInt32(row["DepartmentID"]);
                }
                conn.Close();
            }

            ViewBag.Doctors = GetDoctors();
            ViewBag.Departments = GetDepartments();
            return View(model);
        }

       [HttpPost]
public IActionResult SaveDoctorDepartment(DoctorDepartment model)
{
    // ✅ Manual validation check for dropdowns
    if (model.DoctorID == 0 || model.DepartmentID == 0)
    {
        ModelState.AddModelError("", "Please select Doctor and Department.");
        ViewBag.Doctors = GetDoctors();
        ViewBag.Departments = GetDepartments();
        return View("AddDoctorDepartment", model);
    }

    // ✅ Proceed only if manual validation passed
    string connStr = _configuration.GetConnectionString("DefaultConnection");
    using SqlConnection conn = new SqlConnection(connStr);
    conn.Open();

    SqlCommand cmd = new SqlCommand
    {
        Connection = conn,
        CommandType = CommandType.StoredProcedure
    };

    if (model.DoctorDepartmentID == 0)
    {
        cmd.CommandText = "PR_DoctorDepartment_Insert";
    }
    else
    {
        cmd.CommandText = "PR_DoctorDepartment_UpdateByPK";
        cmd.Parameters.AddWithValue("@DoctorDepartmentID", model.DoctorDepartmentID);
    }

    cmd.Parameters.AddWithValue("@DoctorID", model.DoctorID);
    cmd.Parameters.AddWithValue("@DepartmentID", model.DepartmentID);
            cmd.Parameters.AddWithValue("@Modified", DateTime.Now);
            cmd.Parameters.AddWithValue("@UserID", 1);
            cmd.ExecuteNonQuery();
    conn.Close();

    return RedirectToAction("DoctorDepartmentList");
}






        public IActionResult Delete(int DoctorDepartmentID)
        {
            string connStr = _configuration.GetConnectionString("DefaultConnection");
            using SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("PR_DoctorDepartment_DeleteByPK", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DoctorDepartmentID", DoctorDepartmentID);
            cmd.ExecuteNonQuery();
            conn.Close();

            return RedirectToAction("DoctorDepartmentList");
        }

        private DataTable GetDoctors()
        {
            DataTable dt = new DataTable();
            string connectionstr = _configuration.GetConnectionString("DefaultConnection");
            using SqlConnection conn = new SqlConnection(connectionstr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("PR_Doctor_SelectAll", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = cmd.ExecuteReader();
            dt.Load(reader);
            return dt;
        }

        private DataTable GetDepartments()
        {
            DataTable dt = new DataTable();
            string connectionstr = _configuration.GetConnectionString("DefaultConnection");
            using SqlConnection conn = new SqlConnection(connectionstr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("PR_Department_SelectAll", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = cmd.ExecuteReader();
            dt.Load(reader);
            return dt;
        }
    }
}
