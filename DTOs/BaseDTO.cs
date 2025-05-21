namespace portal.DTOs;

using portal.Models;

public abstract class BaseDTO<TModel>
    where TModel : BaseModel
{
    public int Id { get; set; }
    public string? MainID { get; set; }
}
