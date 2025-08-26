using System.ComponentModel.DataAnnotations;
using Domain.User;

namespace Domain;

public class Organization
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    public ICollection<Place> Places { get; } = new List<Place>();
    
    public ICollection<User.User> Users { get; } = new List<User.User>();
}
