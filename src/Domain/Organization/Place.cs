using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Appointment;

namespace Domain;

public class Place
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public int Capacity { get; set; }
    
    public int OrganizationId { get; set; }

    [ForeignKey(nameof(OrganizationId))]
    public Organization Organization { get; set; }
    
    public ICollection<Slot> Slots { get; set;  } = new List<Slot>();
}