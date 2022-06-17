using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SerenApp.Core.Interfaces;
using SerenApp.Core.Model;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace SerenApp.Web.Areas.Identity.Pages.Account;

public class RegistrationVM
{
    [Required(ErrorMessage = "You must provide a phone number")]
    [DataType(DataType.PhoneNumber)]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "You must provide a password")]
    [DataType(DataType.Password)]
    [MinLength(8)]
    [MaxLength(32)]
    public string Password { get; set; }
}

public class RegisterModel : PageModel
{

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _users;

    public RegisterModel(IHttpContextAccessor httpContextAccessor, IUserRepository users)
    {
        _httpContextAccessor = httpContextAccessor;
        _users = users;
    }

    [BindProperty]
    public RegistrationVM Model { get; set; }

    public string ErrorMessage { get; set; } = "";
    public bool RegistrationSuccessful { get; set; } = false;

    public async Task<IActionResult> OnGetAsync()
    {
        if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
        {
            return Redirect("/");
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        string number = "";
        ErrorMessage = "";

        try
        {
            number = Logic.Auth.ParsePhoneNumber(Model.PhoneNumber);
        }
        catch (PhoneNumbers.NumberParseException e)
        {
            ErrorMessage = e.Message;
            return Page();
        }


        User user = null;

        try
        {
            user = await _users.GetByPhoneNumber(number) ;
        }
        catch(Exception){}

        if (user != null)
        {
            ErrorMessage = "User already exists!";
            return Page();
        }

        var id = Guid.NewGuid();

        user = new User
        {
            ID = id,
            Devices = new List<Device>(),
            PhoneNumber = number,
            SecureContactPhoneNumber = "",
            PasswordHash = Logic.Auth.HashPassword(Model.Password, id.ToByteArray())
        };

        try
        {
            await _users.Insert(user);
        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
            return Page();
        }

        RegistrationSuccessful = true;
        return Page();
    }
}
