using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReactProject.Model
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Username is Required..!")]
        public string Username { get; set; }
        [EmailAddress(ErrorMessage = "Valid Email is required..")]
        [Required(ErrorMessage = "Email is Required..!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is Required..!")]
        public string Password { get; set; }
    }
}
