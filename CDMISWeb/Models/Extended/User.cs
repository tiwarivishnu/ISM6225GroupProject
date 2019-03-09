using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CDMISWeb.Models
{
    [MetadataType(typeof(UsersMetadata))]
    public partial class User
    {
        public string ConfirmPassword { get; set; }
    }

    public class UsersMetadata
    {
        [Display(Name = "First Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "First name required")]
        public string First_Name { get; set; }

        [Display(Name = "Last Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name required")]
        public string Last_Name { get; set; }

        [Display(Name = "Email Id")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email Id required")]
        [DataType(DataType.EmailAddress)]
        public string EmailID { get; set; }

        [Display(Name = "Data Of Birth")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "First name required")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode =true, DataFormatString ="{0:MM/dd/yyyy}")]
        public DateTime DateOfBirth { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minimum 6 characters are required.")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Confirm password and password do not match.")]
        public string ConfirmPassword { get; set; }

    }
}