using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace WebSiteTemplate.Services
{
    public class LocalizedIdentityErrorDescriber : IdentityErrorDescriber
    {
        private readonly IStringLocalizer<SharedResource> _localizer;

        public LocalizedIdentityErrorDescriber(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }

        public override IdentityError DuplicateEmail(string email) => new()
        {
            Code = nameof(DuplicateEmail),
            Description = _localizer["DuplicateEmail", email]
        };

        public override IdentityError PasswordTooShort(int length) => new()
        {
            Code = nameof(PasswordTooShort),
            Description = _localizer["PasswordTooShort", length]
        };

        public override IdentityError PasswordRequiresDigit() => new()
        {
            Code = nameof(PasswordRequiresDigit),
            Description = _localizer["PasswordRequiresDigit"]
        };

        public override IdentityError PasswordRequiresLower() => new()
        {
            Code = nameof(PasswordRequiresLower),
            Description = _localizer["PasswordRequiresLower"]
        };

        public override IdentityError PasswordRequiresUpper() => new()
        {
            Code = nameof(PasswordRequiresUpper),
            Description = _localizer["PasswordRequiresUpper"]
        };

        public override IdentityError PasswordRequiresNonAlphanumeric() => new()
        {
            Code = nameof(PasswordRequiresNonAlphanumeric),
            Description = _localizer["PasswordRequiresNonAlphanumeric"]
        };

        public override IdentityError InvalidToken() => new()
        {
            Code = nameof(InvalidToken),
            Description = _localizer["InvalidToken"]
        };
    }
}