using Developer.API.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Developer.API.Controllers;

[ApiController]
[Route("api/test")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
public class TestController : ControllerBase
{
    /// <summary>
    /// Gets test data for authenticated users.
    /// </summary>
    /// <returns></returns>
    [Authorize(Policy = "User")]
    [HttpGet("data")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    
    public IActionResult TestData()
    {
        var testData = new[] {
            new {
                TestId = "007",
                TestName = "Bond",
                TestInfo = "This is a test"
            },
            new {
                TestId = "320",
                TestName = "Maxwell",
                TestInfo = "Another test!"
            },
            new {
                TestId = "512",
                TestName = "Smart",
                TestInfo = "A great test it will be"
            }
        };

        return Ok(testData);
    }

    /// <summary>
    /// Generates a secret key.
    /// </summary>
    /// <param name="length">Length of key.  Defaults to 256.</param>
    /// <returns></returns>
    [Authorize(Policy = "Admin")]
    [HttpGet("key")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Produces(typeof(string))]
    public IActionResult GenerateSecretKey([FromQuery]int length=256)
    {
        return Ok(SecurityTools.GenerateSecureKey(length));
    }
}
