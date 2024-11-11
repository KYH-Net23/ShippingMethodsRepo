using Microsoft.AspNetCore.Mvc;
using ShippingProvider.DTO.TransitTimeDTOs;
using ShippingProvider.Services;

namespace ShippingProvider.Controllers;

[Route("[controller]")]
[ApiController]
public class ShippingController(IShippingService shippingService) : ControllerBase
{
    [HttpGet("ServicePoints")]
    public async Task<IActionResult> GetNearestServicePoints(
        [FromQuery] string postalCode,
        [FromQuery] int numberOfServicePoints = 3)
    {
        if (string.IsNullOrWhiteSpace(postalCode))
        {
            return BadRequest(new { error = "All address fields are required." });
        }

        try
        {
            var response = await shippingService.GetNearestServicePointsAsync(postalCode, numberOfServicePoints);

            if (response?.ServicePointInformationResponse?.ServicePoints == null ||
                response.ServicePointInformationResponse.ServicePoints.Count == 0)
            {
                return NotFound(new { message = "No service points found." });
            }

            return Ok(response.ServicePointInformationResponse.ServicePoints);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Internal Server Error", details = ex.Message });
        }
    }
    
    [HttpGet("TransitTimes")]
    public async Task<IActionResult> GetTransitTimes([FromQuery] TransitTimeRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var response = await shippingService.GetTransitTimesAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Internal Server Error", details = ex.Message });
        }
    }

}