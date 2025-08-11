using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.User;

public class User
{
    public int Id { get; set; }
    
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }
    
    public int OrganizationId { get; set; }

    [ForeignKey(nameof(OrganizationId))]
    public Organization Organization { get; set; }
    
    public ICollection<Role> Roles { get; } = new List<Role>();

    public ICollection<Appointment.Appointment> Appointments { get; } = new List<Appointment.Appointment>();
}