namespace HahaBuch.Data;

public class VaultEntity
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public IEnumerable<ApplicationUserEntity> Users { get; set; } = new List<ApplicationUserEntity>();
}