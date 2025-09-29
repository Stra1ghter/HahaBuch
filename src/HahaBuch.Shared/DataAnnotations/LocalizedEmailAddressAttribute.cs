using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace HahaBuch.SharedContracts.DataAnnotations;

public sealed class LocalizedEmailAddressAttribute : ValidationAttribute
{
   private readonly EmailAddressAttribute _emailValidator = new();

    public override bool IsValid(object? value)
    {
        // Allow null/empty values (use Required attribute separately if needed)
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            return true;

        return _emailValidator.IsValid(value);
    }
    
    public override string FormatErrorMessage(string name)
    {
        var localizedMessage = GetLocalizedMessage();
        return string.Format(localizedMessage, name);
    }

    private string GetLocalizedMessage()
    {
        var culture = CultureInfo.CurrentUICulture.Name;

        return culture switch
        {
            "de-DE" => "Das {0} Feld muss eine gültige E-Mail-Adresse enthalten.",
            _ => "The {0} field must contain a valid email address."
        };
    }
}