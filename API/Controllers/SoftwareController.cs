using Microsoft.AspNetCore.Mvc;
using ReadFacade.CCP;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SoftwareController : ControllerBase
    {
        private readonly ICCPReadFacade _ccpReadFacade;

        public SoftwareController(ICCPReadFacade ccpReadFacade)
        {
            _ccpReadFacade = ccpReadFacade;
        }

        /// <summary>
        /// Returns all purchaseable software that is offered by ccp in a list.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>List of all purchaseable software</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllPurchaseableSoftware(CancellationToken cancellationToken)
        {
            var software = await _ccpReadFacade.GetSoftwareListAsync(cancellationToken);
            if (!software.Any())
            {
                return NotFound();
            }
            return Ok(software);
        }
    }
}
