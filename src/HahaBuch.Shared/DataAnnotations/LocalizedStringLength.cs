using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace HahaBuch.SharedContracts.DataAnnotations;

public sealed class LocalizedStringLength : StringLengthAttribute
{
    public LocalizedStringLength(int maximumLength) : base(maximumLength)
    {
    }
    
    public LocalizedStringLength(int minimumLength, int maximumLength) : base(maximumLength)
    {
        MinimumLength = minimumLength;
    }

    public override string FormatErrorMessage(string name)
    {
        var localizedMessage = GetLocalizedMessage();
        
        if (MinimumLength > 0)
        {
            return string.Format(localizedMessage, name, MaximumLength, MinimumLength);
        }
        
        return string.Format(localizedMessage, name, MaximumLength);
    }

    private string GetLocalizedMessage()
    {
        var culture = CultureInfo.CurrentUICulture.Name;

        if (MinimumLength > 0)
        {
            return culture switch
            {
                "de-DE" => "Das Feld muss zwischen {2} und {1} Zeichen lang sein.",
                _ => "The field must be between {2} and {1} characters long."
            };
        }
        else
        {
            return culture switch
            {
                "de-DE" => "Das Feld darf nicht länger als {1} Zeichen sein.",
                _ => "The field cannot exceed {1} characters."
            };
        }
    }
}