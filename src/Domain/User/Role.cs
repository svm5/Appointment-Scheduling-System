using System.ComponentModel.DataAnnotations;

namespace Domain.User;

public class Role
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public ICollection<User> Users { get; } = new List<User>();
}