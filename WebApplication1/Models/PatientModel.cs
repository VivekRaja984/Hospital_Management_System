using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Numerics;
using System.Reflection;
using WebApplication1.Controllers;

namespace WebApplication1.Models
{
    public class PatientModel
    {
        public int PatientID { get; set; }
       
        public string Name { get; set; }
        
        public DateTime  DateOfBirth {  get; set; }
        
        public string Gender {  get; set; }
        
        public string Phone {  get; set; } 
        
        public string Address {  get; set; }
        
        public string City { get; set; }
        
        public string State {  get; set; }
        
        public bool IsActive {  get; set; }
        

        
        public DateTime? Modified {  get; set; }
        
        public int UserID {  get; set; } 
        
        public string Email { get; set; }
    }
}
