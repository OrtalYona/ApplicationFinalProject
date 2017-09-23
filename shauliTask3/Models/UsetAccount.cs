using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace shauliTask3.Models
{
    public class UsetAccount
    {
        [Key]
        public int UserId { get; set; }
        [Required(ErrorMessage ="First name is requierd")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is requierd")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is requierd")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "User name is requierd")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is requierd")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password",ErrorMessage ="Please confirm your Password")]
        [DataType(DataType.Password)]
        public string ComfirmPassword { get; set; }
        //[DefaultValue("False")]// check if the problem is here
       public bool IsAdmin { get; set; }


    }
}