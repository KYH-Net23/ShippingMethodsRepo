using System.Text.Json;
using Newtonsoft.Json;
using ShippingProvider.DTO.ServicePointDTOs;
using ShippingProvider.DTO.TransitTimeDTOs;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ShippingProvider.Services;

public class ShippingService(HttpClient httpClient, string secretKey, IConfiguration configuration) : IShippingService
{
    public async Task<ServicePointInformationResponseWrapper?> GetNearestServicePointsAsync(string postalCode, int numberOfServicePoints)
    {
        var baseUrl = configuration["PostNord:BaseUrl"]!;
        var apiUrl = $"{baseUrl}businesslocation/v5/servicepoints/nearest/byaddress?apikey={secretKey}&returnType=json&countryCode=SE&agreementCountry=SE&postalCode={postalCode}&numberOfServicePoints={numberOfServicePoints}&srId=EPSG:4326&context=optionalservicepoint&responseFilter=public&typeId=24,25,54";

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
    
    public async Task<List<TransitTimeResponse>> GetTransitTimesAsync(TransitTimeRequest request)
    {
        var baseUrl = configuration["PostNord:BaseUrl"]!;
        var startTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
        var query = $"{baseUrl}" +
                    $"transport/v1/transittime/addresstoaddress" +
                    $"?apikey={secretKey}" +
                    $"&startTime={Uri.EscapeDataString(startTime)}" +
                    $"&serviceGroup=SE" +
                    $"&originPostCode=14142" +
                    $"&originCountryCode=SE" +
                    $"&destinationPostCode={request.DestinationPostalCode}" +
                    $"&destinationCountryCode=SE";

        var httpResponse = await httpClient.GetAsync(query);
        
        if (!httpResponse.IsSuccessStatusCode)
        {
            var errorContent = await httpResponse.Content.ReadAsStringAsync();
            throw new Exception($"Failed to fetch service points: {errorContent}");
        }

        var responseJson = await httpResponse.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<List<TransitTimeResponse>>(responseJson);
    }

}