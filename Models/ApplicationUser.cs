using Microsoft.AspNetCore.Identity;

namespace SchoolApi.Models
{
    // simple ApplicationUser based on string key (default)
    public class ApplicationUser : IdentityUser
    {
        // add extra profile properties here if you need them, e.g.:
        // public string FullName { get; set; } = string.Empty;
    }
}
