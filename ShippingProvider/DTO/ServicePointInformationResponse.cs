using System.Net;
using System.Text.Json.Serialization;

namespace ShippingProvider.DTO
{
    public class ServicePointInformationResponse
    {
        [JsonPropertyName("servicePoints")]
        public List<ServicePoints> ServicePoints { get; set; } = null!;
    }
}
