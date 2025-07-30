using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using WebApplication1.Models;
using System.Reflection;

namespace WebApplication1.Controllers
{
    public class Patient : Controller
    {
        private readonly IConfiguration _configuration;


        public Patient(IConfiguration configuration)
        {
            _configuration = configuration;
        }

       

        public IActionResult PatientList()
        {
            string connectionstr = _configuration.GetConnectionString("DefaultConnection");

            SqlConnection conn = new SqlConnection(connectionstr);
            conn.Open();

            SqlCommand objCmd = conn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_Patient_SelectAll";

            SqlDataReader objSDR = objCmd.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(objSDR);

            conn.Close();

            return View(dt);
        }
        public IActionResult Delete_patient(int Patientid)
        {
            string connectionstr = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection conn=new SqlConnection(connectionstr))
            {
                conn.Open();
                using (SqlCommand cmd=conn.CreateCommand())
                {
                    cmd.CommandType= CommandType.StoredProcedure;
                    cmd.CommandText = "PR_Patient_DeleteByPk";
                    cmd.Parameters.AddWithValue("@patientID",Patientid);
                    cmd.ExecuteNonQuery();
                }
            }
                return RedirectToAction("PatientList");
        }


        public IActionResult AddPatient(int? PatientId)
        {
            PatientModel model = new PatientModel();

            if (PatientId != null)
            {
                string connectionstr = _configuration.GetConnectionString("DefaultConnection");
                SqlConnection conn = new SqlConnection(connectionstr);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_Patient_SelectByPK";
                cmd.Parameters.AddWithValue("@PatientID", PatientId);

                SqlDataReader reader = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);

                foreach (DataRow row in table.Rows)
                {
                    model.PatientID = Convert.ToInt32(row["PatientID"]);
                    model.Name = row["Name"].ToString();
                    model.Phone = row["Phone"].ToString();
                    model.Email = row["Email"].ToString();
                    model.DateOfBirth = Convert.ToDateTime(row["DateOfBirth"]);
                    model.Gender = row["Gender"].ToString();
                    model.Address = row["Address"].ToString();
                    model.City = row["City"].ToString();
                    model.State = row["State"].ToString();
                }
            }

            return View("AddPatient", model);  
        }

        [HttpPost]
        public IActionResult SavePatient(PatientModel model)
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

                    if (model.PatientID == 0)
                    {
                        cmd.CommandText = "PR_Patient_Insert";
                        cmd.Parameters.AddWithValue("@Name", model.Name);
                        cmd.Parameters.AddWithValue("@Phone", model.Phone);
                        cmd.Parameters.AddWithValue("@Email", model.Email);
                        cmd.Parameters.AddWithValue("@DateOfBirth", model.DateOfBirth);
                        cmd.Parameters.AddWithValue("@Gender",model.Gender);
                        cmd.Parameters.AddWithValue("@Address", model.Address);
                        cmd.Parameters.AddWithValue("@City", model.City);
                        cmd.Parameters.AddWithValue("@State", model.State);
                        cmd.Parameters.AddWithValue("@IsActive", true);
                        cmd.Parameters.AddWithValue("@Modified", DateTime.Now);
                        cmd.Parameters.AddWithValue("@UserID", 1);
                    }
                    else
                    {
                        cmd.CommandText = "PR_Patient_UpdateByPK";
                        cmd.Parameters.AddWithValue("@PatientID",model.PatientID);
                        cmd.Parameters.AddWithValue("@Name", model.Name);
                        cmd.Parameters.AddWithValue("@Phone", model.Phone);
                        cmd.Parameters.AddWithValue("@Email", model.Email);
                        cmd.Parameters.AddWithValue("@DateOfBirth", model.DateOfBirth);
                        cmd.Parameters.AddWithValue("@Gender", model.Gender);
                        cmd.Parameters.AddWithValue("@Address", model.Address);
                        cmd.Parameters.AddWithValue("@City", model.City);
                        cmd.Parameters.AddWithValue("@State", model.State);
                        cmd.Parameters.AddWithValue("@IsActive", true);
                        cmd.Parameters.AddWithValue("@Modified", DateTime.Now);
                        cmd.Parameters.AddWithValue("@UserID", 1);
                    }

                    cmd.ExecuteNonQuery();
                }

                return RedirectToAction("PatientList");
            }

            return View("AddPatient", model);
        }

    }
            
}