using System;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using PhoneNumbers;

namespace SerenApp.Web.Logic;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
sealed public class PhoneNumberValidator : ValidationAttribute
{
    private readonly PhoneNumberUtil phoneNumberUtil = PhoneNumberUtil.GetInstance();

    public override bool IsValid(object value)
    {
        try
        {
            phoneNumberUtil.Parse((string) value, null);
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }
}
