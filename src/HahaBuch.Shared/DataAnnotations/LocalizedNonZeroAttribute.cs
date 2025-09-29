namespace HahaBuch.SharedContracts.DataAnnotations;

using System.Globalization;
using System;
using System.ComponentModel.DataAnnotations;


/// <summary>
/// Validation attribute that ensures a numeric value is not zero.
/// Supports int, long, float, double, decimal, and their nullable variants.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class LocalizedNonZeroAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        // Allow null values - use [Required] if null should not be allowed
        if (value == null)
        {
            return ValidationResult.Success;
        }

        bool isZero = value switch
        {
            int intValue => intValue == 0,
            long longValue => longValue == 0L,
            float floatValue => Math.Abs(floatValue) < float.Epsilon,
            double doubleValue => Math.Abs(doubleValue) < double.Epsilon,
            decimal decimalValue => decimalValue == 0m,
            short shortValue => shortValue == 0,
            byte byteValue => byteValue == 0,
            sbyte sbyteValue => sbyteValue == 0,
            uint uintValue => uintValue == 0u,
            ulong ulongValue => ulongValue == 0ul,
            ushort ushortValue => ushortValue == 0,
            _ => throw new InvalidOperationException($"Unsupported type: {value.GetType().FullName}")
        };

        if (isZero)
        {
            return new ValidationResult(GetLocalizedErrorMessage(), new[] { validationContext.MemberName });
        }

        return ValidationResult.Success;
    }
    
    private string GetLocalizedErrorMessage()
    {
        var culture = CultureInfo.CurrentUICulture.Name;

        return culture switch
        {
            "de-DE" => "Dieser Wert darf nicht 0 sein.",
            _ => "This value must not be zero."
        };
    }
}