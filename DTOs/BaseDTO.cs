namespace portal.DTOs;

using portal.Models;

public abstract class BaseDTO<TModel>
    where TModel : BaseModel
{
    public int? Id { get; set; }
    public string? MainId { get; set; } = "";

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}

public abstract class BaseCreateDTO<TModel>
    where TModel : BaseModel
{
    public string? MainId { get; set; } = "";
}

public abstract class BaseUpdateDTO<TModel>
    where TModel : BaseModel
{
    public string? MainId { get; set; } = "";
}

public abstract class BaseReadDTO<TModel>
    where TModel : BaseModel
{
    public int Id { get; set; }
    public string? MainId { get; set; } = "";

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
