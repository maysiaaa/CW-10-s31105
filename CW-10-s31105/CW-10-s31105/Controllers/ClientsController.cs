using CW_10_s31105.Exceptions;
using CW_10_s31105.Services;
using Microsoft.AspNetCore.Mvc;

namespace CW_10_s31105.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientsController(IDbService service) : ControllerBase
{
    [HttpDelete("{idClient}")]
    public async Task<IActionResult> DeleteStudent([FromRoute] int id)
    {
        try
        {
            await service.RemoveClientAsync(id);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
}