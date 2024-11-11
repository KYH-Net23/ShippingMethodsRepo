namespace ShippingProvider.DTO.TransitTimeDTOs;

public class TransitTimeResponse
{
    public string timeOfDeparture { get; set; }
    public string timeOfArrival { get; set; }
    public ServiceInformation serviceInformation { get; set; }
}