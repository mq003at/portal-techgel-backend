namespace portal.DTOs;

using portal.Models;

public abstract class BaseDTO<TModel>
    where TModel : BaseModel
{
    public int Id { get; set; }
    public string? MainID { get; set; }

    public abstract void UpdateModel(TModel model);

    public void LoadFromModel(TModel model)
    {
        Id = model.Id;
        MainID = model.MainID;
    }
}
