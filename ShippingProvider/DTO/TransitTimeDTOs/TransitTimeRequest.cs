using System.ComponentModel.DataAnnotations;

namespace ShippingProvider.DTO.TransitTimeDTOs
{
    public class TransitTimeRequest
    {
        [Required(ErrorMessage = "DestinationPostalCode is required.")]
        [Display(Name = "DestinationPostalCode", Description = "Postal code of the destination address")]
        public string DestinationPostalCode { get; set; } = null!;
    }
}