namespace portal.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public abstract class BaseModel
{
    [Key]
    public int Id { get; set; } // Primary Key, auto-generated
    public string MainId { get; set; } = string.Empty;

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; }
}

public abstract class BaseModelWithOnlyId
{
    [Key]
    public int Id { get; set; }
}


