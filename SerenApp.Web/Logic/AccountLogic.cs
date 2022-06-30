using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using PhoneNumbers;
using SerenApp.Core.Interfaces;
using SerenApp.Core.Model;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace SerenApp.Web.Logic
{
    public class AccountLogic
    {
        private static readonly PhoneNumberUtil phoneNumberUtil = PhoneNumberUtil.GetInstance();
        private readonly IUserRepository users;
        private readonly IAdminRepository admins;
        private readonly IHttpContextAccessor contextAccessor;

        public const string UserRole = "USER";
        public const string AdminRole = "ADMIN";

        public AccountLogic(IUserRepository users, IAdminRepository admins, IHttpContextAccessor contextAccessor)
        {
            this.users = users;
            this.admins = admins;
            this.contextAccessor = contextAccessor;         
        }

        public static string ParsePhoneNumber(string numberStr)
        {
            var number = phoneNumberUtil.Parse(numberStr, null);
            return phoneNumberUtil.Format(number, PhoneNumberFormat.E164);
        }

        private string HashPassword(string password, byte[] salt)
        {
            var hash = KeyDerivation.Pbkdf2(
                password,
                salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8);
            return Convert.ToBase64String(hash);
        }

        private bool ComparePassword(string password, string hash, byte[] salt)
        {
            var hashed = HashPassword(password, salt);
            return hashed == hash;
        }

        public async Task<(bool,string)> LoginUserAsync(string phone, string password)
        {
            try
            {
                phone = ParsePhoneNumber(phone);
            }
            catch (NumberParseException e)
            {
                return (false, e.Message);
            }

            User user = null;
            try
            {
                user = await users.GetByPhoneNumber(phone);
            }
            catch (Exception) { }

            if (user == null || !ComparePassword(password, user.PasswordHash, user.ID.ToByteArray()))
            {
                return (false, "Wrong user or password");
            }

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.PhoneNumber));
            claims.Add(new Claim(ClaimTypes.Role, UserRole));

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await contextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties { });

            return (true, "");
        }

        public async Task<(bool, string)> LoginAdminAsync(string username, string password)
        {

            Admin user = null;
            try
            {
                user = await admins.GetByUsername(username);
            }
            catch (Exception) { }

            if (user == null || !ComparePassword(password, user.PasswordHash, user.ID.ToByteArray()))
            {
                return (false, "Wrong user or password");
            }

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.Username));
            claims.Add(new Claim(ClaimTypes.Role, AdminRole));

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await contextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties { });

            return (true, "");
        }

        public async Task<(bool, string)> RegisterUserAsync(string phone, string password)
        {
            try
            {
                phone = ParsePhoneNumber(phone);
            }
            catch (NumberParseException e)
            {
                return (false, e.Message);
            }


            User user = null;

            try
            {
                user = await users.GetByPhoneNumber(phone);
            }
            catch (Exception) { }

            if (user != null)
            {
                return (false, "User already exists!");
            }

            var id = Guid.NewGuid();

            user = new User
            {
                ID = id,
                Devices = new List<Device>(),
                PhoneNumber = phone,
                SecureContactPhoneNumber = "",
                PasswordHash = HashPassword(password, id.ToByteArray())
            };

            try
            {
                await users.Insert(user);
            }
            catch (Exception)
            {
                return (false, "An unknown error has occurred");
            }

            return (true, "");
        }
    }
}
