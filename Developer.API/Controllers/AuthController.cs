using Developer.API.Auth;
using Developer.API.Models.Api;
using Developer.API.Persistence.Identities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Developer.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : Controller
{
    private UserAuthentication _userAuth;

    public AuthController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _userAuth = new UserAuthentication(userManager, configuration);
    }

    /// <summary>
    /// Registers a new user and creates an account in the sample application.
    /// </summary>
    /// <returns>Returns a JWT token.</returns>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var result = await _userAuth.RegisterUserAsync(model.Username, model.Password);

        if (result == null)
            return StatusCode(500, "Failed to register user in application.");
        
        return (result?.Token != null) ? 
            Ok(new { result.Value.Token }) : BadRequest(result!.Value.Errors);
    }

    /// <summary>
    /// Logs into the Sample Application with user name and password.
    /// </summary>
    /// <param name="model"></param>
    /// <returns>Returns a JWT token on success.</returns>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var token = await _userAuth.LoginUserAsync(model.Username, model.Password);

        return (token != null) ?
            Ok(new { token }) :
            Unauthorized("Username or Password is invalid.");
    }
}
