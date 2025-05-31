using CW_10_s31105.Exceptions;
using CW_10_s31105.Services;
using Microsoft.AspNetCore.Mvc;

namespace CW_10_s31105.Controllers;

[ApiController]
[Route("[controller]")]
public class TripsController(IDbService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetTripsDetails([FromRoute] int page)
    {
        return Ok(await service.GetTripsDetailsAsync(page));
    }

    [HttpPost("{idTrip}/clients")]
    public async Task<IActionResult> AssignClientToTrip(string clientPesel, int idTrip)
    {
        try
        {
            await service.AssignClientToTripAsync(clientPesel, idTrip);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}