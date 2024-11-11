using System.Text.Json.Serialization;

namespace ShippingProvider.DTO
{
    public class VisitingAddress
    {
        [JsonPropertyName("countryCode")]
        public string CountryCode { get; set; } = null!;

        [JsonPropertyName("city")]
        public string City { get; set; } = null!;

        [JsonPropertyName("streetName")]
        public string StreetName { get; set; } = null!;

        [JsonPropertyName("streetNumber")]
        public string StreetNumber { get; set; } = null!;

        [JsonPropertyName("postalCode")]
        public string PostalCode { get; set; } = null!;
    }
}
