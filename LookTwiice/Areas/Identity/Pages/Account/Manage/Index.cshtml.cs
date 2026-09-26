// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.RateLimiting;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using LookTwiice.Data;
using LookTwiice.Models;
using LookTwiice.Resources;
using PhoneNumbers;
using System.Linq;

namespace LookTwiice.Areas.Identity.Pages.Account.Manage;


public class IndexModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public IndexModel(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [TempData]
    public string? StatusMessage { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; } = default!;

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class InputModel
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        /// 
        public string CountryCode { get; set; } = "+90";

        public string? PhoneNumber { get; set; }

        [Required(ErrorMessageResourceName = "NameRequired", ErrorMessageResourceType = typeof(ValidationMessages))]
        [StringLength(50, ErrorMessageResourceName = "NameTooLong", ErrorMessageResourceType = typeof(ValidationMessages))]
        public string? Name { get; set; }

        [Required(ErrorMessageResourceName = "SurnameRequired", ErrorMessageResourceType = typeof(ValidationMessages))]
        [StringLength(50, ErrorMessageResourceName = "NameTooLong", ErrorMessageResourceType = typeof(ValidationMessages))]
        public string? Surname { get; set; }

        
    }



    private async Task LoadAsync(ApplicationUser user)
    {
        var userName = await _userManager.GetUserNameAsync(user);
        var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

        Username = userName;

        var countryCode = "+90";
        var localNumber = phoneNumber;

        if (!string.IsNullOrWhiteSpace(phoneNumber))
        {
            try
            {
                var phoneUtil = PhoneNumberUtil.GetInstance();
                var parsedNumber = phoneUtil.Parse(phoneNumber, null);

                if (parsedNumber.CountryCode > 0)
                {
                    countryCode = $"+{parsedNumber.CountryCode}";
                    localNumber = parsedNumber.NationalNumber.ToString();
                }
            }
            catch (NumberParseException)
            {
                // Eski/geçersiz bir numara varsa mevcut değeri bozmuyoruz.
            }
        }

        Input = new InputModel
        {
            CountryCode = countryCode,
            PhoneNumber = localNumber,
            Name = user.Name,
            Surname = user.Surname
        };
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        await LoadAsync(user);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        if (!ModelState.IsValid)
        {
            await LoadAsync(user);
            return Page();
        }

        var phoneUtil = PhoneNumberUtil.GetInstance();

        PhoneNumber parsedPhone;

        try
        {
            parsedPhone = phoneUtil.Parse(
                Input.PhoneNumber,
                Input.CountryCode
            );
        }
        catch (NumberParseException)
        {
            ModelState.AddModelError(
                "Input.PhoneNumber",
                "Geçerli bir telefon numarası giriniz."
            );

            return Page();
        }

        if (!phoneUtil.IsValidNumber(parsedPhone))
        {
            ModelState.AddModelError(
                "Input.PhoneNumber",
                "Geçerli bir telefon numarası giriniz."
            );

            return Page();
        }

        var fullPhone = phoneUtil.Format(
            parsedPhone,
            PhoneNumberFormat.E164
        );

        var currentPhone =
            await _userManager.GetPhoneNumberAsync(user);

        if (fullPhone != currentPhone)
        {
            var setPhoneResult =
                await _userManager.SetPhoneNumberAsync(
                    user,
                    fullPhone
                );

            if (!setPhoneResult.Succeeded)
            {
                StatusMessage =
                    "Telefon numarası güncellenirken bir hata oluştu.";

                return RedirectToPage();
            }
        }


        user.Name = Input.Name;
        user.Surname = Input.Surname;
        await _userManager.UpdateAsync(user);

        await _signInManager.RefreshSignInAsync(user);
        StatusMessage = "Your profile has been updated";
        return RedirectToPage();
    }
}
