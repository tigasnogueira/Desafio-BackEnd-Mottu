namespace BikeRentalSystem.Core.Models;

public abstract class EntityBase
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;

    public void Update()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    public void ToggleIsDeleted()
    {
        IsDeleted = !IsDeleted;
    }
}
