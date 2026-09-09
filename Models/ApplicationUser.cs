using Microsoft.AspNetCore.Identity;

namespace HelpDeskTicketing.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string FullName { get; set; } = string.Empty;
    }
}