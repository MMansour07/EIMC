using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using eInvoicing.Web.Views.User;

namespace eInvoicing.Web.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessageResourceType = typeof(UserResources),ErrorMessageResourceName = "RequiredFieldMsg")]
        [EmailAddress(ErrorMessageResourceType = typeof(UserResources), ErrorMessageResourceName = "InvalidEmailMsg")]
        public string Email { get; set; }
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(UserResources), ErrorMessageResourceName = "RequiredFieldMsg")]
        public string PhoneNumber { get; set; }
        public string Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(UserResources), ErrorMessageResourceName = "RequiredFieldMsg")]
        
        public string Title { get; set; }

        [Required(ErrorMessageResourceType = typeof(UserResources), ErrorMessageResourceName = "RequiredFieldMsg")]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        
        public string Password { get; set; }

        [DataType(DataType.Password)]
       
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessageResourceType = typeof(UserResources), ErrorMessageResourceName = "NonmatchingPasswordMsg")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(UserResources), ErrorMessageResourceName = "RequiredFieldMsg")]
        
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(UserResources), ErrorMessageResourceName = "RequiredFieldMsg")]
        
        public string LastName { get; set; }
        public string FullName { get; set; }

        [Required(ErrorMessageResourceType = typeof(UserResources), ErrorMessageResourceName = "RequiredFieldMsg")]
        //[Display(ResourceType = typeof(UserResources), Name = "Roles")]
        public List<string> Roles { get; set; }
        public string BusinessGroupId { get; set; }
        public string SlashSeparatedRoles { get; set; }

    }
    public class EditViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please select at least one role")]
        public List<string> Roles { get; set; }
        public string SlashSeparatedRoles { get; set; }
    }

    public class SettingViewModel
    {
        public string ProductionLoginURL { get; set; }
        public bool APIsEnvironment { get; set; }
        public bool Environment { get; set; }
        public bool ETAHubEnvironment { get; set; }
        public bool TypeVersion { get; set; }
        public string ProductionApiURL { get; set; }
        public string ProductionClientId { get; set; }
        public string ProductionClientSecret { get; set; }
        public string ProductionAppKey { get; set; }
        public string ProductionAppId { get; set; }
        public string PreProductionLoginURL { get; set; }
        public string PreProductionApiURL { get; set; }
        public string PreProductionClientId { get; set; }
        public string PreProductionClientSecret { get; set; }
        public string PreProductionAppKey { get; set; }
        public string PreProductionAppId { get; set; }
        public string DevSignerURL { get; set; }
        public string ProductionSignerURL { get; set; }
        public string DevAPIURL { get; set; }
        public string TokenPin { get; set; }
        public string BusinessGroupId { get; set; }
        public string ProductionInternalAPIURL { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
    public class AccountVM
    {
        public RegisterViewModel RegisterViewModel { get; set; }
        public LoginViewModel LoginViewModel { get; set; }
        public ForgotPasswordViewModel ForgotPasswordViewModel { get; set; }
    }

    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage ="Required Field.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Required Field.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Required Field.")]
        public List<string> Permissions { get; set; }
    }

    public class SelectRoleEditorViewModel
    {
        public SelectRoleEditorViewModel() { }

        // Update this to accept an argument of type ApplicationRole:

        public bool Selected { get; set; }

        [Required]
        public string RoleName { get; set; }

        // Add the new Description property:
        public string Description { get; set; }
    }

    public class EditRoleViewModel
    {
        public string Id { get; set; }

        public string OriginalRoleName { get; set; }
        [Required(ErrorMessage ="Please fill role name field.")]
        public string RoleName { get; set; }
        [Required(ErrorMessage = "Please fill description field.")]
        public string Description { get; set; }

        public EditRoleViewModel() { }
    }
}
