using EFCoreDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCoreDatabase.Contexts;

internal class MyContext : DbContext
{
    public DbSet<ProjectRevision> ProjectRevisions => Set<ProjectRevision>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            @"Server=localhost,5433;Initial Catalog=EFCoreDatabaseTest2;User Id=sa;Password=Pass@word;TrustServerCertificate=true");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<RevisionCalculationData>().HasKey(x => x.ProjectRevisionId);
        modelBuilder.Entity<RevisionCalculationData>().HasOne<ProjectRevision>()
            .WithOne(x => x.RevisionCalculationData)
            .HasForeignKey<RevisionCalculationData>(x => x.ProjectRevisionId);

        modelBuilder.Entity<EngineOperationInformation>().HasKey(x => x.RevisionCalculationDataId);
        modelBuilder.Entity<EngineOperationInformation>()
            .HasDiscriminator<string>(EngineOperationInformationConfig.DiscriminatorColumnName)
            .HasValue<ContinuousEngineOperationInformation>(EngineOperationInformationConfig
                .ContinuousEngineOperationInformation)
            .HasValue<FlexibleEngineOperationInformation>(EngineOperationInformationConfig
                .FlexibleEngineOperationInformation);

        modelBuilder.Entity<EngineOperationInformation>().HasOne<RevisionCalculationData>()
            .WithOne(x => x.EngineOperation)
            .HasForeignKey<EngineOperationInformation>(x => x.RevisionCalculationDataId);
    }
}