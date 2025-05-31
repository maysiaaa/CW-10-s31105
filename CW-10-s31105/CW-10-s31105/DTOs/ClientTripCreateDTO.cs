using System.ComponentModel.DataAnnotations;

namespace CW_10_s31105.DTOs;

public class ClientTripCreateDTO
{
    [Required]
    public int IdClient { get; set; }

    [Required]
    public int IdTrip { get; set; }

    [Required]
    public int RegisteredAt { get; set; }

    public int? PaymentDate { get; set; }
}