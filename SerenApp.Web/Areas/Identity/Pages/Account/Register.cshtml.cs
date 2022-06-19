using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using SerenApp.Web.Logic;

namespace SerenApp.Web.Areas.Identity.Pages.Account;

public class UserRegistrationVM
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

    [Required(ErrorMessage = "You must repeat the password")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Passwords don't match.")]
    public string ConfirmPassword { get; set; }
}

public class RegisterModel : PageModel
{

    private readonly IHttpContextAccessor contextAccessor;
    private readonly AccountLogic logic;

    public RegisterModel(IHttpContextAccessor httpContextAccessor, AccountLogic logic)
    {
        this.contextAccessor = httpContextAccessor;
        this.logic = logic;
    }

    [BindProperty]
    public UserRegistrationVM Model { get; set; }

    public string ErrorMessage { get; set; } = "";
    public bool RegistrationSuccessful { get; set; } = false;

    public async Task<IActionResult> OnGetAsync()
    {
        return contextAccessor.HttpContext.User.Identity.IsAuthenticated ? Redirect("/") : Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        
        var results = new List<ValidationResult>();
        if (!Validator.TryValidateObject(Model, new ValidationContext(Model), results, true))
        {
            ErrorMessage = string.Join("\n", results.Select(x => x.ErrorMessage));
            return Page();
        }

        (RegistrationSuccessful, ErrorMessage) = await logic.RegisterUserAsync(Model.PhoneNumber, Model.Password);

        return Page();
    }
}
