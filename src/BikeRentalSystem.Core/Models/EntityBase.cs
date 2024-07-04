namespace BikeRentalSystem.Core.Models;

public abstract class EntityBase
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }

    public EntityBase()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        IsDeleted = false;
    }

    public void Update()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    public void IsDeletedToggle()
    {
        IsDeleted = !IsDeleted;
    }
}

[AttributeUsage(AttributeTargets.Property)]
public class IgnoreOnUpdateAttribute : Attribute
{
}
