using Microsoft.EntityFrameworkCore;

namespace EFCoreDatabase.Contexts;

internal class MyContext : DbContext
{
    public DbSet<MainEntity> MainEntities => Set<MainEntity>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            @"Server=localhost,5433;Initial Catalog=EFCoreDatabaseTest2;User Id=sa;Password=Pass@word;TrustServerCertificate=true");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<EntityWithInheritance>().HasKey(x => x.MainEntityId);
        modelBuilder.Entity<EntityWithInheritance>()
            .HasDiscriminator<string>(EntityWithInheritanceConfig.DiscriminatorColumnName)
            .HasValue<EntityWithInheritanceOne>(EntityWithInheritanceConfig.EntityWithInheritanceOne)
            .HasValue<EntityWithInheritanceTwo>(EntityWithInheritanceConfig.EntityWithInheritanceTwo);

        modelBuilder.Entity<EntityWithInheritance>().HasOne<MainEntity>()
            .WithOne(x => x.EntityWithInheritance)
            .HasForeignKey<EntityWithInheritance>(x => x.MainEntityId);
    }
}

public class MainEntity
{
    public Guid Id { get; set; }
    public virtual EntityWithInheritance? EntityWithInheritance { get; set; }
}

public static class EntityWithInheritanceConfig
{
    public const string EntityWithInheritanceOne = "EntityWithInheritanceOne";
    public const string EntityWithInheritanceTwo = "EntityWithInheritanceTwo";
    public const string DiscriminatorColumnName = "Discriminator";
}

public abstract class EntityWithInheritance
{
    public Guid MainEntityId { get; set; }
    public int MyNumber { get; set; }
}

public class EntityWithInheritanceOne : EntityWithInheritance
{
}

public class EntityWithInheritanceTwo : EntityWithInheritance
{
    public int MyOtherNumber { get; set; }
}