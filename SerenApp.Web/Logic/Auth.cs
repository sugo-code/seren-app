using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace SerenApp.Web.Logic
{
    public static class Auth
    {
        public static string ParsePhoneNumber(string numberStr)
        {
            var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();

            var number = phoneNumberUtil.Parse(numberStr, null);
            return phoneNumberUtil.Format(number, PhoneNumbers.PhoneNumberFormat.E164);
        }

        public static string HashPassword(string password, byte[] salt)
        {
            var hash = KeyDerivation.Pbkdf2(
                password,
                salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8);
            return Convert.ToBase64String(hash);
        }

        public static bool ComparePassword(string password, string hash, byte[] salt)
        {
            var hashed = HashPassword(password, salt);
            return hashed == hash;
        }
    }
}
