using SerenApp.Core.Interfaces;
using SerenApp.Core.Model;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SerenApp.Web.Areas.Identity.Pages.Account;

public class LoginVM
{
    //[Required(ErrorMessage = "You must provide a phone number")]
    //[DataType(DataType.PhoneNumber)]
    //[Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }

    //[Required(ErrorMessage = "You must provide a password")]
    //[DataType(DataType.Password)]
    //[MinLength(8)]
    //[MaxLength(32)]
    public string Password { get; set; }
}

public class LoginModel : PageModel
{

    private IHttpContextAccessor ContextAccessor;
    private IUserRepository Users;

    public LoginModel(IHttpContextAccessor ContextAccessor, IUserRepository Users)
    {
        this.ContextAccessor = ContextAccessor;
        this.Users = Users;
    }

    [BindProperty]
    public LoginVM Model { get; set; }

    public string ErrorMessage { get; set; }

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
            user = await Users.GetByPhoneNumber(number);
        }
        catch (Exception e) { }

        if (user == null || !Logic.Auth.ComparePassword(Model.Password, user.PasswordHash, user.ID.ToByteArray()))
        {
            ErrorMessage = "Wrong user or password";
            return Page();
        }

        var claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.Name, user.ID.ToString()));
        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()));
        claims.Add(new Claim(ClaimTypes.MobilePhone, user.PhoneNumber));
        claims.Add(new Claim(ClaimTypes.Role, "USER"));

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties { };

        await ContextAccessor.HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return Redirect("/");
    }

    public async Task<IActionResult> OnGetAsync()
    {
        if (ContextAccessor.HttpContext.User.Identity.IsAuthenticated)
        {
            return Redirect("/");
        }
        return Page();
    }
}
