using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace HahaBuch.SharedContracts.DataAnnotations;

public sealed class LocalizedCompareAttribute : CompareAttribute
{
    public LocalizedCompareAttribute(string otherProperty) : base(otherProperty)
    {
    }

    public override string FormatErrorMessage(string name)
    {
        var localizedMessage = GetLocalizedMessage();
        return string.Format(localizedMessage, name, OtherPropertyDisplayName ?? OtherProperty);
    }

    private string GetLocalizedMessage()
    {
        var culture = CultureInfo.CurrentUICulture.Name;

        return culture switch
        {
            "de-DE" => "Eingaben stimmen nicht überein.",
            _ => "Inputs do not match."
        };
    }
}