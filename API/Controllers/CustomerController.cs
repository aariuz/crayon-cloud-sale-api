using API.Authentication;
using API.Contracts;
using Common.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadFacade.Crayon;
using WriteFacade.Crayon;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICrayonReadFacade _crayonReadFacade;
        private readonly ICrayonWriteFacade _crayonWriteFacade;

        public CustomerController(ICrayonReadFacade crayonReadFacade, ICrayonWriteFacade crayonWriteFacade)
        {
            _crayonReadFacade = crayonReadFacade;
            _crayonWriteFacade = crayonWriteFacade;
        }

        /// <summary>
        /// Returns all accounts linked to the customer.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>List of all accounts connected to the customer.</returns>
        [HttpGet("Accounts")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAccountsForCustomer(CancellationToken cancellationToken)
        {
            var customerId = User.FindFirst("customerId")?.Value;
            var accounts = await _crayonReadFacade.GetAllAccountsForCustomerAsync(int.Parse(customerId), cancellationToken);
            if (!accounts.Any())
            {
                return NotFound();
            }
            return Ok(accounts);
        }

        /// <summary>
        /// Creates a new customer using the provided email, password and name in the request, provided the email is not already in use.
        /// </summary>
        /// <param name="request">The request containing email, name and password</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A new customer object.</returns>
        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateNewCustomerRequest request, CancellationToken cancellationToken)
        {
            var result = PasswordHelper.HashPassword(request.Password);

            var newCustomerRequest = new NewCustomerRequest
            {
                Email = request.Email,
                Name = request.Name,
                PasswordHash = result.HashBase64,
                Salt = result.SaltBase64
            };

            return Ok(await _crayonWriteFacade.CreateNewCustomer(newCustomerRequest, cancellationToken));
        }
    }
}
