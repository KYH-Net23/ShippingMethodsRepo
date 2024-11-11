using System.Text.Json;
using ShippingProvider.DTO;

namespace ShippingProvider.Services;

public class ShippingService(HttpClient httpClient, string secretKey, IConfiguration configuration) : IShippingService
{
    public async Task<ServicePointInformationResponseWrapper?> GetNearestServicePointsAsync(string postalCode, int numberOfServicePoints)
    {
        var baseUrl = configuration["PostNord:BaseUrl"]!;
        var apiUrl = $"{baseUrl}servicepoints/nearest/byaddress?apikey={secretKey}&returnType=json&countryCode=SE&agreementCountry=SE&postalCode={postalCode}&numberOfServicePoints={numberOfServicePoints}&srId=EPSG:4326&context=optionalservicepoint&responseFilter=public&typeId=24,25,54";

        var httpResponse = await httpClient.GetAsync(apiUrl);

        if (!httpResponse.IsSuccessStatusCode)
        {
            var errorContent = await httpResponse.Content.ReadAsStringAsync();
            throw new Exception($"Failed to fetch service points: {errorContent}");
        }

        var responseJson = await httpResponse.Content.ReadAsStringAsync();
        
        return JsonSerializer.Deserialize<ServicePointInformationResponseWrapper>(
            responseJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}