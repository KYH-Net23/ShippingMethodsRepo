using ShippingProvider.DTO.ServicePointDTOs;
using ShippingProvider.DTO.TransitTimeDTOs;

namespace ShippingProvider.Services;

public interface IShippingService
{
    Task<ServicePointInformationResponseWrapper?> GetNearestServicePointsAsync(string postalCode,
        int numberOfServicePoints);

    Task<List<TransitTimeResponse>> GetTransitTimesAsync(TransitTimeRequest request);
}