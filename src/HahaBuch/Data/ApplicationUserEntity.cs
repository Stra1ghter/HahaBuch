using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HahaBuch.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUserEntity : IdentityUser<Guid>
{
    public ApplicationUserEntity()
    {
    }

    public ApplicationUserEntity(VaultEntity existingVaultEntity)
    {
        VaultEntityId = existingVaultEntity.Id;
        VaultEntity = existingVaultEntity;
    }
    
   public void InitializeVault()
    {
        VaultEntity = new VaultEntity
        {
            Name = $"{UserName}'s Vault"
        };
        VaultEntityId = VaultEntity.Id;
    } 
    
    // Note: must be nullable for UserManager to work
    public Guid? VaultEntityId { get; set; }

    public VaultEntity? VaultEntity { get; set; } = default!;
}