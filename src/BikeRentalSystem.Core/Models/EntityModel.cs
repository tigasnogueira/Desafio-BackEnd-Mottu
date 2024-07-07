namespace BikeRentalSystem.Core.Models;

public abstract class EntityModel
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }

    public EntityModel()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        IsDeleted = false;
    }

    public void Delete()
    {
        IsDeleted = true;
    }

    public void Update()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}
