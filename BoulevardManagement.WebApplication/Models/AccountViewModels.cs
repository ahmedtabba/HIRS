using BoulevardManagement.DTO;
using BoulevardManagement.WebApplication.Resources;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TripleA.Utilities;

namespace BoulevardManagement.WebApplication.Models
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
        [Display(Name = "Email", ResourceType = typeof(LoginResource))]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessageResourceType = typeof(LoginResource), ErrorMessageResourceName = "EmailRequired")]
        [Display(Name = "Email", ResourceType = typeof(LoginResource))]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(LoginResource), ErrorMessageResourceName = "PasswordRequired")]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(LoginResource))]
        public string Password { get; set; }

        [Display(Name = "RememberMe", ResourceType = typeof(LoginResource))]
        public bool RememberMe { get; set; }
    }

    public class UserRoleViewModel
    {
        public UserRoleViewModel()
        {
            Roles = new List<TreeViewItemModel>();
        }

        [Required]
        public string UserId { get; set; }

        [Required]
        public List<TreeViewItemModel> Roles { get; set; }

        public List<string> RolesIds { get; set; }

    }

    public class UserGroupViewModel
    {

        [Required]
        public string UserId { get; set; }

        [Required]
        public List<ApplicationGroup> UserGroups { get; set; }

        public List<string> GroupIds { get; set; }

    }

    public class RegisterViewModel
    {
        public string Id { get; set; }
        [Required]
        [Display(Name = "Employee Full Name")]
        public string FullName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email/ User name")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone No")]
        public string PhoneNumber { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public JobRole JobRole { get; set; }
        public int? DepartmentId { get; set; }
        public DateTime? CreationDate{ get; set; }

        public bool IsLockedOut { get; set; }

        public IList<int> SelectedQuickActions { get; set; }
        public IList<ApplicationGroup> UserGroups { get; set; }
        public IList<string> GroupIds { get; set; }
        public IList<int> SelectedNotificationGroups { get; set; }
        public Gender Gender { get; set; }
     //   public string UserColor { get; set; }
        public string PhotoUrl { get; set; }
        public bool HasPhoto { get; set; }
        public String Descreption { get; set; }
        public IList<NotificationGroupDTO> NotificationGroups { get; set; }

        public IList<System.Web.Mvc.SelectListItem> AvailableChannels { get; set; }
        public RegisterViewModel()
        {
            Id = "";
            GroupIds = new List<string>();
            AvailableChannels = new List<System.Web.Mvc.SelectListItem>();
            UserGroups = new List<ApplicationGroup>();
            SelectedNotificationGroups = new List<int>();
            NotificationGroups = new List<NotificationGroupDTO>();
            SelectedQuickActions = new List<int>();
        }

    }

    public class UsersViewModel
    {
        public string Id { get; set; }
        [Required]
        public string FullName { get; set; }
        public string Email { get; set; }

        public string PhotoUrl { get; set; }
        public bool HasPhoto { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string RoleNames { get; set; }
        public string RoleIds { get; set; }

        public string GroupNames { get; set; }

        public string GroupIds { get; set; }

        public bool IsLockedOut { get; set; }
        public JobRole JobRole { get; set; }
        public string JobRoleDescription { get { return JobRole.GetEnumDescription(); } }
        public string NotificationGroupsNames { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public DateTime? CreationDate { get; set; }
        public Gender Gender { get; set; }
        // public string UserColor { get; set; }
        public string Descreption { get; set; }
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
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
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
}
