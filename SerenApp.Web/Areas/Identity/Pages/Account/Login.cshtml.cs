using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SerenApp.Web.Logic;

namespace SerenApp.Web.Areas.Identity.Pages.Account;

public class UserLoginVM
{
    [Required(ErrorMessage = "You must provide a phone number")]
    [DataType(DataType.PhoneNumber)]
    [Display(Name = "Phone Number")]
    [PhoneNumberValidator(ErrorMessage = "Invalid international phone number")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "You must provide a password")]
    [DataType(DataType.Password)]
    [MinLength(8)]
    [MaxLength(32)]
    public string Password { get; set; }
}

public class AdminLoginVM
{
    [Required(ErrorMessage = "You must provide a username")]
    [Display(Name = "Username")]
    public string Username { get; set; }

    [Required(ErrorMessage = "You must provide a password")]
    [DataType(DataType.Password)]
    [MinLength(8)]
    [MaxLength(32)]
    public string Password { get; set; }
}

public class LoginModel : PageModel
{
    private readonly AccountLogic logic;
    private readonly IHttpContextAccessor contextAccessor;

    public LoginModel(AccountLogic logic, IHttpContextAccessor contextAccessor)
    {
        this.logic = logic;
        this.contextAccessor = contextAccessor;
    }

    [BindProperty]
    public UserLoginVM UserModel { get; set; }

    [BindProperty]
    public AdminLoginVM AdminModel { get; set; }

    public string ErrorMessage { get; set; }

    public bool LastLoginWasAdmin { get; set; } = false;

    public async Task<IActionResult> OnGetAsync()
    {
        return contextAccessor.HttpContext.User.Identity.IsAuthenticated ? Redirect("/") : Page();
    }

    public async Task<IActionResult> OnPostLoginUserAsync()
    {
        LastLoginWasAdmin = false;

        var results = new List<ValidationResult>();
        if (!Validator.TryValidateObject(UserModel, new ValidationContext(UserModel), results, true))
        {
            ErrorMessage = string.Join("\n", results.Select(x => x.ErrorMessage));
            return Page();
        }

        (bool success, ErrorMessage) = await logic.LoginUserAsync(UserModel.PhoneNumber, UserModel.Password);
        return success ? Redirect("/") : Page();
    }

    public async Task<IActionResult> OnPostLoginAdminAsync()
    {
        LastLoginWasAdmin = true;

        var results = new List<ValidationResult>();
        if (!Validator.TryValidateObject(AdminModel, new ValidationContext(AdminModel), results, true))
        {
            ErrorMessage = string.Join("\n", results.Select(x => x.ErrorMessage));
            return Page();
        }

        (bool success, ErrorMessage) = await logic.LoginAdminAsync(AdminModel.Username, AdminModel.Password);
        return success ? Redirect("/") : Page();
    }
}
