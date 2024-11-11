using System.Text.Json.Serialization;

namespace ShippingProvider.DTO.ServicePointDTOs
{
    public class ServicePoints
    {
        [JsonPropertyName("name")] 
        public string Name { get; set; } = null!;

        [JsonPropertyName("servicePointId")]
        public string ServicePointId { get; set; } = null!;

        [JsonPropertyName("visitingAddress")]
        public VisitingAddress VisitingAddress { get; set; } = null!;
    }
}
