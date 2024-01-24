using System.ComponentModel.DataAnnotations;

namespace OnlineVoteApplication.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Please enter Username.")]
        [Display(Name = "Username")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Username is not valid")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter Password.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Password \"{0}\" must have {2} characters", MinimumLength = 8)]
        [RegularExpression(@"^([a-zA-Z0-9@*#]{8,15})$", ErrorMessage = "Password must contain: Minimum 8 characters, " +
            "at least 1 UpperCase Alphabet, 1 LowerCase Alphabet, 1 Number, and 1 Special Character")]
        public string Password { get; set; }
    }
}
