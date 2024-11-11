using ShippingProvider.DTO.ServicePointDTOs;
using ShippingProvider.DTO.TransitTimeDTOs;

namespace ShippingProvider.Services;

public interface IShippingService
{
    Task<ServicePointInformationResponseWrapper?> GetNearestServicePointsAsync(string postalCode,
        int numberOfServicePoints);

    Task<List<TransitTime>> GetTransitTimesAsync(TransitTimeRequest request);
}