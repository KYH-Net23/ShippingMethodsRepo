using System.ComponentModel.DataAnnotations;

namespace ShippingProvider.DTO.TransitTimeDTOs
{
    public class TransitTimeRequest
    {
        [Required(ErrorMessage = "OriginPostalCode is required.")]
        [Display(Name = "OriginPostalCode", Description = "Postal code of the origin address")]
        public string OriginPostalCode { get; set; } = null!;

        [Required(ErrorMessage = "DestinationPostalCode is required.")]
        [Display(Name = "DestinationPostalCode", Description = "Postal code of the destination address")]
        public string DestinationPostalCode { get; set; } = null!;

        [Required(ErrorMessage = "StartTime is required.")]
        [Display(Name = "StartTime", Description = "Start time for the transit (default: current time)")]
        public string StartTime { get; set; } = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
    }
}