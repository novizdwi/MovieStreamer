using System.ComponentModel.DataAnnotations;

namespace MovieStreamer.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email or Phone")]
        public string? UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        public string? ReturnUrl { get; set; }
        public bool RememberLogin { get; set; }
    }

    public class RegisterViewModel
    {
        public string? Email { get; set; }
        public string? Phone { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password), Display(Name="Confirm Password")]
        public string ConfirmPassword { get; set; }
    }

    public class SettingViewModel
    {
        public string? Id { get; set; }
        [Display(Name = "Name")]
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [DataType(DataType.Password), Display(Name = "Confirm Password")]
        public string? ConfirmPassword { get; set; }
    }
}
