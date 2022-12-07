using Microsoft.EntityFrameworkCore;

namespace EFCoreDatabase.Contexts;

internal class MyContext : DbContext
{
    public DbSet<MainEntity> ProjectRevisions => Set<MainEntity>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            @"Server=localhost,5433;Initial Catalog=EFCoreDatabaseTest2;User Id=sa;Password=Pass@word;TrustServerCertificate=true");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ChildEntity>().HasKey(x => x.MainEntityId);
        modelBuilder.Entity<ChildEntity>().HasOne<MainEntity>()
            .WithOne(x => x.ChildEntity)
            .HasForeignKey<ChildEntity>(x => x.MainEntityId);

        modelBuilder.Entity<EntityWithInheritance>().HasKey(x => x.ChildEntityId);
        modelBuilder.Entity<EntityWithInheritance>()
            .HasDiscriminator<string>(EntityWithInheritanceConfig.DiscriminatorColumnName)
            .HasValue<EntityWithInheritanceOne>(EntityWithInheritanceConfig.EntityWithInheritanceOne)
            .HasValue<EntityWithInheritanceTwo>(EntityWithInheritanceConfig.EntityWithInheritanceTwo);

        modelBuilder.Entity<EntityWithInheritance>().HasOne<ChildEntity>()
            .WithOne(x => x.EntityWithInheritance)
            .HasForeignKey<EntityWithInheritance>(x => x.ChildEntityId);
    }
}

public class ChildEntity
{
    public Guid MainEntityId { get; set; }
    public virtual EntityWithInheritance? EntityWithInheritance { get; set; }
}

public class MainEntity
{
    public Guid Id { get; set; }
    public virtual ChildEntity? ChildEntity { get; set; }
}

public static class EntityWithInheritanceConfig
{
    public const string EntityWithInheritanceOne = "EntityWithInheritanceOne";
    public const string EntityWithInheritanceTwo = "EntityWithInheritanceTwo";
    public const string DiscriminatorColumnName = "Discriminator";
}

public abstract class EntityWithInheritance
{
    public Guid ChildEntityId { get; set; }
    public int MyNumber { get; set; }
}

public class EntityWithInheritanceOne : EntityWithInheritance
{
}

public class EntityWithInheritanceTwo : EntityWithInheritance
{
    public int MyOtherNumber { get; set; }
}