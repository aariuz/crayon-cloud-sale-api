using API.Authentication;
using API.Contracts;
using Microsoft.AspNetCore.Mvc;
using ReadFacade.Crayon;
using WriteFacade.Crayon;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly ICrayonReadFacade _crayonReadFacade;
        private readonly ICrayonWriteFacade _crayonWriteFacade;

        public AuthenticationController(ITokenService tokenService,
            ICrayonReadFacade crayonReadFacade, ICrayonWriteFacade crayonWriteFacade)
        {
            _tokenService = tokenService;
            _crayonReadFacade = crayonReadFacade;
            _crayonWriteFacade = crayonWriteFacade;
        }

        /// <summary>
        /// Creates a JWT for the user, provided that the email and password are correct.
        /// </summary>
        /// <param name="request">The request containing email and password</param>
        /// <returns>Object containing a new JWT and a refresh token</returns>
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            var user = await _crayonReadFacade.GetCustomerByEmailAsync(request.Email, cancellationToken);
            if (user is null || !PasswordHelper.VerifyPassword(request.Password, user.PasswordHash, user.Salt))
                return Unauthorized();

            var accessToken = _tokenService.CreateAccessToken(user.Email, user.Id);
            var refreshToken = _tokenService.CreateRefreshToken();

            await _crayonWriteFacade.UpdateCustomerTokenAsync(user.Id, refreshToken, cancellationToken);

            return Ok(new { accessToken, refreshToken });
        }

        /// <summary>
        /// Creating a new JWT if the provided refresh token is still valid.
        /// </summary>
        /// <param name="request">Request containing the current refresh token.</param>
        /// <returns>Object containing the new access token and refresh token.</returns>
        [HttpPost("Refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var user = await _crayonReadFacade.GetCustomerByTokenAsync(request.RefreshToken, cancellationToken);
            if (user is null || user.RefreshTokenExpiry < DateTime.UtcNow)
                return Unauthorized();

            var newAccessToken = _tokenService.CreateAccessToken(user.Email, user.Id);
            var newRefreshToken = _tokenService.CreateRefreshToken();

            await _crayonWriteFacade.UpdateCustomerTokenAsync(user.Id, newRefreshToken, cancellationToken);

            return Ok(new { accessToken = newAccessToken, refreshToken = newRefreshToken });
        }
    }
}
