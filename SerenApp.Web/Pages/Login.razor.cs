using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SerenApp.Core.Interfaces;
using SerenApp.Core.Model;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace SerenApp.Web.Pages
{
    public class LoginVM
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

    public class LoginModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _users;

        public LoginModel(IHttpContextAccessor httpContextAccessor, IUserRepository users)
        {
            _httpContextAccessor = httpContextAccessor;
            _users = users;
        }


        [BindProperty]
        public LoginVM Model { get; set; }

        public string ErrorMessage { get; set; } = "";

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
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }

            var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();
            string parsed = "";

            try
            {
                var number = phoneNumberUtil.Parse(Model.PhoneNumber, null);
                parsed = phoneNumberUtil.Format(number, PhoneNumbers.PhoneNumberFormat.E164);
            }
            catch (PhoneNumbers.NumberParseException e)
            {
                ErrorMessage = e.Message;
                return Page();
            }

            var user = await _users.GetByPhoneNumber(parsed);
            if (user != null)
            {
                ErrorMessage = "Wrong user or password";
                return Page();
            }

            byte[] hashed = KeyDerivation.Pbkdf2(
                password: Model.Password,
                salt: user.ID.ToByteArray(),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8);

            if(hashed == user.PasswordHash)
            {
                ErrorMessage = "Wrong user or password";
                return Page();
            }

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()));
            claims.Add(new Claim(ClaimTypes.MobilePhone, user.PhoneNumber));
            claims.Add(new Claim(ClaimTypes.Role, "USER"));

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties{ };

            await _httpContextAccessor.HttpContext.SignInAsync(
                       CookieAuthenticationDefaults.AuthenticationScheme,
                       new ClaimsPrincipal(claimsIdentity),
                       authProperties);

            return Page();

        }
    }
}
