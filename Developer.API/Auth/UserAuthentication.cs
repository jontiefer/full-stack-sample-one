using Developer.API.Persistence.Identities.Models;
using Developer.API.Security;
using Microsoft.AspNetCore.Identity;

namespace Developer.API.Auth;

public class UserAuthentication
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public UserAuthentication(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<(string? Token, IEnumerable<IdentityError>? Errors)?> RegisterUserAsync(string username, string password)
    {
        try
        {
            var user = new ApplicationUser { UserName = username };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");

                var token = await SecurityTools.GenerateJwtTokenAsync(user, _userManager, _configuration);

                return (token, null);
            }

            return (null, result.Errors);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error logging user into the application.  Error Message: {ex.Message}");

            return null;
        }
    }

    public async Task<string?> LoginUserAsync(string username, string password)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user != null && await _userManager.CheckPasswordAsync(user, password))
            {
                var token = await SecurityTools.GenerateJwtTokenAsync(user, _userManager, _configuration);

                return token;
            }

            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error registering user into application.  Error Message: {ex.Message}");

            return null;
        }
    }
}
