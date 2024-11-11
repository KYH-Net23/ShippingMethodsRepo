using System.Text.Json.Serialization;

namespace ShippingProvider.DTO.ServicePointDTOs
{
    public class ServicePointInformationResponseWrapper
    {
        [JsonPropertyName("servicePointInformationResponse")]
        public ServicePointInformationResponse ServicePointInformationResponse { get; set; } = null!;
    }
}
