using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Appointment;

public class Slot
{
    public int Id { get; set; }
    
    [Required]
    public DateTime From { get; set; }
    
    [Required]
    public DateTime To { get; set; }
    
    public int PlaceId { get; set; }

    [ForeignKey(nameof(PlaceId))]
    public Place Place { get; set; }
    
    public Appointment? Appointment { get; set; }
}