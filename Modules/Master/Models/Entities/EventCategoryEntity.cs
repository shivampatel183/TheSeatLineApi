using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("EventCategories")]
public class EventCategory
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(120)]
    public string Slug { get; set; } = null!;

    // Navigation – a category can have many events
    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}


