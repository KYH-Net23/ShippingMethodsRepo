using System.Text.Json.Serialization;

namespace ShippingProvider.DTO
{
    public class ServicePointInformationResponseWrapper
    {
        [JsonPropertyName("servicePointInformationResponse")]
        public ServicePointInformationResponse ServicePointInformationResponse { get; set; } = null!;
    }
}
