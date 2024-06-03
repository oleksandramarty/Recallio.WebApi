using Recallio.Domain.Models.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Recallio.Domain;

  public class DataContext : IdentityDbContext<User, Role, Guid>
  {
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Role>(entity => { entity.ToTable("Roles", "Recallio.User"); });
      modelBuilder.Entity<User>(entity => { entity.ToTable("Users", "Recallio.User"); });

      var cascadeFKs = modelBuilder.Model.GetEntityTypes()
        .SelectMany(t => t.GetForeignKeys())
        .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

      foreach (var fk in cascadeFKs)
        fk.DeleteBehavior = DeleteBehavior.Restrict;

      base.OnModelCreating(modelBuilder);
    }
  }