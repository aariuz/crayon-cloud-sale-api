using API.Contracts;
using Common.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WriteFacade.Crayon;

namespace API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ICrayonWriteFacade _crayonWriteFacade;

        public AccountController(ICrayonWriteFacade crayonWriteFacade)
        {
            _crayonWriteFacade = crayonWriteFacade;
        }

        /// <summary>
        /// Purchase software using a list of orders containing the ccp software id togheter with a quantity for each software to purchase and the account id.
        /// </summary>
        /// <param name="request">The order request containing a list of objects that contain ccp software id together with a quantity and account id.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>List of all the newly purchased subscriptions.</returns>
        [HttpPost("PurchaseSoftware")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PurchaseaSoftware([FromBody] SoftwareOrderRequest request, CancellationToken cancellationToken)
        {
            var customerId = User.FindFirst("customerId")?.Value;
            return Ok(await _crayonWriteFacade.PurchaseSoftwareAsync(int.Parse(customerId), request.AccountId, request, cancellationToken));
        }

        /// <summary>
        /// Update the current subscription, either removing existing licenses or adding new ones depending on the request and existing subscription.
        /// </summary>
        /// <param name="request">The request containing the subscription id and the new quantity of licenses.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The updated subscription.</returns>
        [HttpPost("UpdateSubscription")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateSubscription([FromBody] UpdateSubscriptionRequest request, CancellationToken cancellationToken)
        {
            var customerId = User.FindFirst("customerId")?.Value;
            return Ok(await _crayonWriteFacade.UpdateNumberOfSubscriptionLicensesAsync(int.Parse(customerId), request.SubscriptionId, request.AmountOfLicenses, cancellationToken));
        }

        /// <summary>
        /// Cancel the subscription and all its licenses using subscription id.
        /// </summary>
        /// <param name="subscriptionId">The subscription id for the customers account.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>200 OK status.</returns>
        [HttpPost("CancelSubscription/{subscriptionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateSubscription(int subscriptionId, CancellationToken cancellationToken)
        {
            var customerId = User.FindFirst("customerId")?.Value;
            await _crayonWriteFacade.CancelSubscriptionForAccountAsync(int.Parse(customerId), subscriptionId, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Extend the subscriptions lifetime.
        /// </summary>
        /// <param name="request">The request containing the subscription id together with a new date.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The updated subscription.</returns>
        [HttpPost("ExtendSubscription")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ExtendSubscription([FromBody] ExtendSubscriptionRequest request, CancellationToken cancellationToken)
        {
            var customerId = User.FindFirst("customerId")?.Value;
            return Ok(await _crayonWriteFacade.ExtendSubscriptionDateAsync(int.Parse(customerId), request.SubscriptionId, request.NewDate, cancellationToken));
        }

        /// <summary>
        /// Creates a new account for customer with the provided account name and optional description.
        /// </summary>
        /// <param name="request">The request containing the desired account name and optional description.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The newly created account.</returns>
        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateNewAccount([FromBody] CreateNewAccountRequest request, CancellationToken cancellationToken)
        {
            var customerId = User.FindFirst("customerId")?.Value;

            var newAccountRequest = new NewAccountRequest
            {
                CustomerId = int.Parse(customerId),
                Description = request.Description,
                Name = request.Name
            };
            
            return Ok(await _crayonWriteFacade.CreateNewAccountForCustomer(newAccountRequest, cancellationToken));
        }
    }
}
