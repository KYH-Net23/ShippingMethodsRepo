using ShippingProvider.DTO;

namespace ShippingProvider.Services;

public interface IShippingService
{
    Task<ServicePointInformationResponseWrapper?> GetNearestServicePointsAsync(string postalCode,
        int numberOfServicePoints);
}