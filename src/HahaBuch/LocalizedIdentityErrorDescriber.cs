using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace HahaBuch;

public class LocalizedIdentityErrorDescriber(IStringLocalizer<LocalizedIdentityErrorDescriber> loc) : IdentityErrorDescriber
{
   public override IdentityError PasswordRequiresUniqueChars(int uniqueChars) => new IdentityError
   {
      Code = nameof(PasswordRequiresUniqueChars), Description = String.Format(loc["PasswordUniqueChars"], uniqueChars)
   };

   public override IdentityError RecoveryCodeRedemptionFailed() => new IdentityError
   {
      Code = nameof(RecoveryCodeRedemptionFailed), Description = loc["RecoveryCodeRedemptionFailed"]
   };

   public override IdentityError UserNotInRole(string role) => new IdentityError()
   {
      Code = nameof(UserNotInRole), Description = String.Format(loc["UserNotInRole"], role)
   };
   
   public override IdentityError DefaultError() => new IdentityError { Code = nameof(DefaultError), Description = loc["DefaultError"] };
   
   public override IdentityError ConcurrencyFailure() => new IdentityError { Code = nameof(ConcurrencyFailure), Description = loc["ConcurrencyFailure"] };
   
   public override IdentityError PasswordMismatch() => new IdentityError { Code = nameof(PasswordMismatch), Description = loc["PasswordMismatch"] };
   
   public override IdentityError InvalidToken() => new IdentityError { Code = nameof(InvalidToken), Description = loc["InvalidToken"] };
   
   public override IdentityError LoginAlreadyAssociated() => new IdentityError { Code = nameof(LoginAlreadyAssociated), Description = loc["LoginAlreadyAssociated"] };
   
   public override IdentityError InvalidEmail(string? email) => new IdentityError
   {
      Code = nameof(InvalidEmail), Description = String.Format(loc["InvalidEmail"], email)
   };
   
   public override IdentityError InvalidRoleName(string? role) => new IdentityError
   {
      Code = nameof(InvalidRoleName), Description = String.Format(loc["InvalidRoleName"], role)
   };
   
   public override IdentityError InvalidUserName(string? userName) => new IdentityError
   {
      Code = nameof(InvalidUserName), Description = String.Format(loc["InvalidUserName"], userName)
   };
   
   public override IdentityError DuplicateUserName(string userName) => new IdentityError
   {
      Code = nameof(DuplicateUserName), Description = String.Format(loc["DuplicateUserName"], userName)
   };
   
   public override IdentityError DuplicateEmail(string email) => new IdentityError
   {
      Code = nameof(DuplicateEmail), Description = String.Format(loc["DuplicateEmail"], email)
   };
   
   public override IdentityError DuplicateRoleName(string role) => new IdentityError
   {
      Code = nameof(DuplicateRoleName), Description = String.Format(loc["DuplicateRoleName"], role)
   };
   
   public override IdentityError UserAlreadyHasPassword() => new IdentityError { Code = nameof(UserAlreadyHasPassword), Description = loc["UserAlreadyHasPassword"] };
   
   public override IdentityError UserLockoutNotEnabled() => new IdentityError { Code = nameof(UserLockoutNotEnabled), Description = loc["UserLockoutNotEnabled"] };
   public override IdentityError UserAlreadyInRole(string role) => new IdentityError
   {
      Code = nameof(UserAlreadyInRole), Description = String.Format(loc["UserAlreadyInRole"], role)
   };
   
   public override IdentityError PasswordTooShort(int length) => new IdentityError
   {
      Code = nameof(PasswordTooShort), Description = String.Format(loc["PasswordTooShort"], length)
   };
   
   public override IdentityError PasswordRequiresNonAlphanumeric() => new IdentityError
   {
      Code = nameof(PasswordRequiresNonAlphanumeric), Description = loc["PasswordRequiresNonAlphanumeric"]
   };
   
   public override IdentityError PasswordRequiresDigit() => new IdentityError { Code = nameof(PasswordRequiresDigit), Description = loc["PasswordRequiresDigit"] };
   
   public override IdentityError PasswordRequiresLower() => new IdentityError { Code = nameof(PasswordRequiresLower), Description = loc["PasswordRequiresLower"] };
   
   public override IdentityError PasswordRequiresUpper() => new IdentityError
   {
      Code = nameof(PasswordRequiresUpper), Description = loc["PasswordRequiresUpper"]
   };
}