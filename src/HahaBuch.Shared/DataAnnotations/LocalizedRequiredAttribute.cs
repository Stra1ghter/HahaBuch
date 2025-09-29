using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace HahaBuch.SharedContracts.DataAnnotations;

public sealed class LocalizedRequiredAttribute : RequiredAttribute
{
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
            "de-DE" => "Das {0} Feld muss ausgefüllt werden.",
            _ => "The {0} field is required."
        };
    }
}