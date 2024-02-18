using Microsoft.AspNetCore.Identity;

namespace Developer.API.Persistence.Identities.Models;

public class ApplicationRole : IdentityRole
{
    public ApplicationRole()
    {
    }

    public ApplicationRole(string roleName, string description)
    : base(roleName)
    {
        Description = description;
    }

    public string? Description { get; set; } 
}

