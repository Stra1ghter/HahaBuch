using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HahaBuch.Data;

public static class SetupDatabaseExtensions
{
   // /// <summary>
   // /// Apply pending migrations and seed the database.
   // /// Seed a superuser that should be changed after the first application start.
   // /// </summary>
   // /// <param name="host">The web application host.</param>
   public static async Task SetupDatabaseAsync(this IHost host)
   {
      // We could use a seed method of the dbcontext, but this approach uses the specific hashing algorithm of the user manager.
      IServiceProvider serviceProvider = host.Services;
      using var scope = serviceProvider.CreateScope();
      var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
      var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUserEntity>>();
      
      await context.Database.MigrateAsync();
   
      if (!context.Users.Any())
      {
         ApplicationUserEntity superUserEntity = new();
         await userManager.SetUserNameAsync(superUserEntity, "superuser");
         await userManager.SetEmailAsync(superUserEntity, "admin@admin.com");
         var result = await userManager.CreateAsync(superUserEntity, "SecurePassword123!");
         
         var user = await context.Users.FirstAsync();
         superUserEntity.EmailConfirmed = true;
         superUserEntity.InitializeVault();
         await context.SaveChangesAsync();
   
         if (result.Succeeded)
         {
            await userManager.AddClaimAsync(superUserEntity, new System.Security.Claims.Claim("IsSuperUser", "true"));
         }
      }
   
   }
}