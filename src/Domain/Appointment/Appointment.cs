using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Appointment;

public class Appointment
{
    public int Id { get; set; }
    
    public int SlotId { get; set; }
    public Slot Slot { get; set; }
    
    public ICollection<User.User> Users { get; } = new List<User.User>();
}