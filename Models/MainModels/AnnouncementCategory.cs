namespace portal.Models;

using System.ComponentModel.DataAnnotations;

public class AnnouncementCategory : BaseModel
{
    [Required]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool isRestricted {get; set; } = false;

    public List<int>? AllowedEmployeeIds {get; set; }

    // public ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();
}
