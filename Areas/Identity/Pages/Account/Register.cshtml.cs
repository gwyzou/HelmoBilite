// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using HelmoBilite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using HelmoBilite.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis;

namespace HelmoBilite.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<BasicUser> _signInManager;
        private readonly UserManager<BasicUser> _userManager;
        private readonly IUserStore<BasicUser> _userStore;
        private readonly IUserEmailStore<BasicUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _appDbContext;

        public RegisterModel(
            UserManager<BasicUser> userManager,
            IUserStore<BasicUser> userStore,
            SignInManager<BasicUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender, ApplicationDbContext dbContext )
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _appDbContext = dbContext;

            Input = new InputModel();
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            //TODO Add  for each values of input + add db
            string formName = Request.Form["formName"];

            BasicUser user = CreateUser(formName);
            
            if (user != null)
            {
                return await AddToDb(user);
            }
            //Never supposed to arrive there
            return RedirectToPage("~/Shared/Error");

            /*
            
            TODO � laisser disponnible en commentaire au cas ou tant que register pas fini
                
                    returnUrl ??= Url.Content("~/");
                    await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                    await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                    var result = await _userManager.CreateAsync(user, Input.Password);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created a new account with password.");

                        var userId = await _userManager.GetUserIdAsync(user);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                        }
                        else
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                

            
            */


            //TODO si l'utilisateur est bien cr�� mettre input vide pour avoir la possibilit� de cr�er un nouveau compte par la suite (si on reste dans la m�me session
        }

        private async Task<IActionResult> AddToDb(BasicUser user)
        {
            //AAA111aaaa#
            
            await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

            var result = await _userManager.CreateAsync(user, Input.Password);
            var addRoleResult = await _userManager.AddToRoleAsync(user, Request.Form["formName"]);
            
            if (result.Succeeded && addRoleResult.Succeeded)
            {
                _logger.LogInformation("User created a new account with password");
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToPage("~/Home/Index");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }

        [Obsolete]
        private async Task<IActionResult> addToDBWithContext<T>(T user) where T : BasicUser
        {
            //It is supposed that the state is valid at this point

            switch (user)
            {
                //TODO Verif en fonction des cas si d�j� utilis�: email , nom de compagnie , Matricul , ...
                case Client client:
                    if (_userManager.FindByEmailAsync(user.Email).Result != null)
                        return LocalRedirect("/Share/Error");
                    _appDbContext.Client?.Add(client);
                    break;
                case Driver driver:
                    if(_userManager.FindByEmailAsync(user.Email).Result != null)
                        return LocalRedirect("/Share/Error");
                    _appDbContext.Driver?.Add(driver);
                    break;
                case Dispa dispatcher:
                    if(_userManager.FindByEmailAsync(user.Email).Result != null)
                        return LocalRedirect("/Share/Error");
                    _appDbContext.Dispa?.Add(dispatcher);
                    break;
                default:
                    throw new ArgumentException("Invalid user type.");
            }

            // Save the changes to the database
            await _appDbContext.SaveChangesAsync();
            //redirect to home page
            return LocalRedirect("/Home/Index");
        }
        private BasicUser CreateUser(string formName)
        {
            if (!Input.ValidateModel())
            {
                // Return null if input model is invalid
                return null;
            }

            switch (formName)
            {
                case "Client" when Input.ModelClient.ValidateModel():
                    {
                        Client user = CreateInstance<Client>();
                        user.Email = Input.Email;
                        user.CompanyName = Input.ModelClient.NameCompany;
                        user.CompanyAddress = Input.ModelClient.Adress;
                        user.FirstName = Input.Name;
                        user.LastName = Input.Surname;
                        return user;
                    }
                case "Driver" when Input.ModelDriver.ValidateModel():
                    {
                        Driver user = CreateInstance<Driver>();
                        user.Email = Input.Email;
                        user.Mat = Input.ModelDriver.Matricul;
                        user.FirstName = Input.Name;
                        user.LastName = Input.Surname;
                        user.DriverLicense = Input.ModelDriver.Licence;
                        return user;
                    }
                case "Dispatcher" when Input.ModelDispatcher.ValidateModel():
                    {
                        Dispa user = CreateInstance<Dispa>();
                        user.Email = Input.Email;
                        user.Mat = Input.ModelDispatcher.Matricul;
                        user.FirstName = Input.Name;
                        user.LastName = Input.Surname;
                        user.Diploma = Input.ModelDispatcher.Studies;
                        return user;
                    }
                default:
                    // Return null if formName is not recognized or if validation fails
                    return null;
            }
        }
        private T CreateInstance<T>() where T : new()
        {
            try
            {
                return Activator.CreateInstance<T>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{typeof(T).Name}'. " +
                    $"Ensure that '{typeof(T).Name}' is not an abstract class and has a parameterless constructor.");
            }
        }

        private IUserEmailStore<BasicUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
                throw new NotSupportedException("The default UI requires a user store with email support.");
            return (IUserEmailStore<BasicUser>)_userStore;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            [Required]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required]
            [Display(Name = "Surname")]
            public string Surname { get; set; }
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
                MinimumLength = 8)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public InputModelClient ModelClient  { get; set; }
            public InputModelDispatcher ModelDispatcher { get; set; }
            public InputModelDriver ModelDriver { get; set; }

            public InputModel()
            {
                ModelClient = new InputModelClient();
                ModelDispatcher = new InputModelDispatcher();
                ModelDriver = new InputModelDriver();
            }
            public bool ValidateModel()
            {
                var context = new ValidationContext(this, serviceProvider: null, items: null);
                var results = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(this, context, results, validateAllProperties: true);

                return isValid;
            }

        }
        public class InputModelClient
        {
            [Required]
            [Display(Name = "Name Company")]
            public string NameCompany { get; set; }

            [Required]
            [Display(Name = "Head office")]
            public string Adress { get; set; }

            public bool ValidateModel()
            {
                var context = new ValidationContext(this, serviceProvider: null, items: null);
                var results = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(this, context, results, validateAllProperties: true);

                return isValid;
            }
        }
        public class InputModelHelmo
        {
            [Required]
            [Display(Name = "Matricul")]
            [RegularExpression("^[A-Z][0-9]{6}$", ErrorMessage = "Invalid matricule format! It should be like \"A02457\" (upper case Letter followed by 6 digits)")]
            public string Matricul { get; set; }

        }
        public class InputModelDriver : InputModelHelmo
        {
            [Required]
            [Display(Name = "Licence")]
            public DriverLicence Licence { get; set; }

            public bool ValidateModel()
            {
                var context = new ValidationContext(this, serviceProvider: null, items: null);
                var results = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(this, context, results, validateAllProperties: true);

                return isValid;
            }
        }
        public class InputModelDispatcher : InputModelHelmo
        {
            [Required]
            [Display(Name = "Studies")]
            public Diploma Studies { get; set; }

            public bool ValidateModel()
            {
                var context = new ValidationContext(this, serviceProvider: null, items: null);
                var results = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(this, context, results, validateAllProperties: true);

                return isValid;
            }
        }
    }
   
}
