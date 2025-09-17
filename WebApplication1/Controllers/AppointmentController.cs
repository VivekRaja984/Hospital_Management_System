using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using WebApplication1.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication1.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IConfiguration _configuration;

        public AppointmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult AppointmentList(string statusFilter, int? doctorId, int? patientId)
        {
            string connectionstr = _configuration.GetConnectionString("DefaultConnection");

            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionstr))
            {
                conn.Open();

                SqlCommand objCmd = conn.CreateCommand();
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "PR_Appointment_SelectAll";

                
                if (!string.IsNullOrEmpty(statusFilter))
                    objCmd.Parameters.AddWithValue("@Status", statusFilter);
                if (doctorId.HasValue)
                    objCmd.Parameters.AddWithValue("@DoctorID", doctorId.Value);
                if (patientId.HasValue)
                    objCmd.Parameters.AddWithValue("@PatientID", patientId.Value);

                SqlDataReader objSDR = objCmd.ExecuteReader();
                dt.Load(objSDR);
            }
            
            ViewBag.Doctors = GetDoctors();   
            ViewBag.Patients = GetPatients(); 
            ViewBag.StatusList = new List<string> { "Pending", "Completed" };

            return View(dt);
        }


        public IActionResult AddAppointment(int? AppointmentID)
        {
            AppointmentModel model = new AppointmentModel();

            if (AppointmentID != null)
            {
                string connectionstr = _configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection conn = new SqlConnection(connectionstr))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand("PR_Appointment_SelectByPK", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@AppointmentID", AppointmentID);

                    SqlDataReader reader = command.ExecuteReader();
                    DataTable table = new DataTable();
                    table.Load(reader);

                    foreach (DataRow row in table.Rows)
                    {
                        model.AppointmentID = Convert.ToInt32(row["AppointmentID"]);
                        model.DoctorID = Convert.ToInt32(row["DoctorID"]);
                        model.PatientID = Convert.ToInt32(row["PatientID"]);
                        model.AppointmentDate = Convert.ToDateTime(row["AppointmentDate"]);
                        model.AppointmentStatus = row["AppointmentStatus"].ToString();
                        model.Description = row["Description"].ToString();
                        model.SpecialRemarks = row["SpecialRemarks"].ToString();
                        model.TotalConsultedAmount = Convert.ToDecimal(row["TotalConsultedAmount"]);
                    }
                }
            }

            ViewBag.Doctors = GetDoctors();
            ViewBag.Patients = GetPatients();
            
            return View(model);
        }

        public IActionResult Delete(int AppointmentID)
        {
            string connectionstr = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection conn = new SqlConnection(connectionstr))
            {
                conn.Open();
                using (SqlCommand sqlCommand = conn.CreateCommand())
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "PR_Appointment_DeleteByPK";
                    sqlCommand.Parameters.AddWithValue("@AppointmentID", AppointmentID);
                    sqlCommand.ExecuteNonQuery();
                }
            }
            return RedirectToAction("AppointmentList");
        }
        [HttpPost]
        public IActionResult SaveAppointment(AppointmentModel model)
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

                    if (model.AppointmentID == 0)
                    {
                        cmd.CommandText = "PR_Appointment_Insert";
                    }
                    else
                    {
                        cmd.CommandText = "PR_Appointment_UpdateByPK";
                        cmd.Parameters.AddWithValue("@AppointmentID", model.AppointmentID);
                    }

                    cmd.Parameters.AddWithValue("@DoctorID", model.DoctorID);
                    cmd.Parameters.AddWithValue("@PatientID", model.PatientID);
                    cmd.Parameters.AddWithValue("@AppointmentDate", model.AppointmentDate);
                    cmd.Parameters.AddWithValue("@AppointmentStatus", model.AppointmentStatus);
                    cmd.Parameters.AddWithValue("@Description", model.Description ?? "");
                    cmd.Parameters.AddWithValue("@SpecialRemarks", model.SpecialRemarks ?? "");
                    cmd.Parameters.AddWithValue("@Modified", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UserID", 1); 
                    cmd.Parameters.AddWithValue("@TotalConsultedAmount", model.TotalConsultedAmount);

                    cmd.ExecuteNonQuery();
                }

                return RedirectToAction("AppointmentList");
            }

            
            ViewBag.Doctors = GetDoctors();
            ViewBag.Patients = GetPatients();
            
            return View("AddAppointment", model);
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

        private DataTable GetPatients()
        {
            DataTable dt = new DataTable();
            string connectionstr = _configuration.GetConnectionString("DefaultConnection");
            using SqlConnection conn = new SqlConnection(connectionstr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("PR_Patient_SelectAll", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = cmd.ExecuteReader();
            dt.Load(reader);
            return dt;
        }
    }
}
