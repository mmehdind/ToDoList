using System.ComponentModel.DataAnnotations;

namespace TodoList.ViewModels
{

public class RegisterViewModel
{
    // [Required(ErrorMessage = "نام الزامی است")]
    // public string FirstName { get; set; } = "";

    // [Required(ErrorMessage = "نام خانوادگی الزامی است")]
    // public string LastName { get; set; } = "";

    [Required(ErrorMessage = "شماره تماس الزامی است")]
    [Phone(ErrorMessage = "شماره تماس معتبر نیست")]
    public string PhoneNumber { get; set; } = "";

    [Required(ErrorMessage = "جنسیت الزامی است")]
    public Gender Gender { get; set; }

    [Required(ErrorMessage = "نام کاربری الزامی است")]
    public string UserName { get; set; } = "";

    [Required(ErrorMessage = "ایمیل الزامی است")]
    [EmailAddress(ErrorMessage = "ایمیل معتبر نیست")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "رمز عبور الزامی است")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";

    [Required(ErrorMessage = "تکرار رمز عبور الزامی است")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "رمز عبور و تکرار آن یکسان نیست")]
    public string ConfirmPassword { get; set; } = "";
}
}
