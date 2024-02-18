using Developer.API.Persistence.Identities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Developer.API.Persistence.Contexts;

public class SampleAppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public SampleAppDbContext(DbContextOptions<SampleAppDbContext> options)
        : base(options)
    {
        
    }
}
