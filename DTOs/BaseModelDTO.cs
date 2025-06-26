namespace portal.DTOs;

using portal.Models;

public abstract class BaseModelDTO
{
    public int Id { get; set; }
    public string? MainId { get; set; } = "";

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}

public abstract class BaseModelCreateDTO
{
}

public abstract class BaseModelUpdateDTO
{
}

public abstract class BaseModelWithOnlyIdDTO
{
    public long Id { get; set; }
}
public abstract class BaseModelWithOnlyIdCreateDTO
{
    // No properties needed, as this is just a marker for creation
}
public abstract class BaseModelWithOnlyIdUpdateDTO
{
    // No properties needed, as this is just a marker for updates
}
// This class is used to define a base model with only an Id property.
