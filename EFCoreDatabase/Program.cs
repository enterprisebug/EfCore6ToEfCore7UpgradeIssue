using EFCoreDatabase.Contexts;
using EFCoreDatabase.Models;
using Microsoft.EntityFrameworkCore;

await using (var ctx = new MyContext())
{
    await ctx.Database.EnsureDeletedAsync();
    await ctx.Database.EnsureCreatedAsync();
}

await using (var ctx2 = new MyContext())
{
    var projectRevsision = new ProjectRevision
    {
        RevisionCalculationData = new RevisionCalculationData
        {
            EngineOperation = new ContinuousEngineOperationInformation
            {
                StartsPerYear = 1,
                OperatingHourRange = 2,
                RunTimePerYearAndEngine = 3
            }
        }
    };

    await ctx2.ProjectRevisions.AddAsync(projectRevsision);
    await ctx2.SaveChangesAsync();
}

await using (var ctx3 = new MyContext())
{
    var projectRevision = await ctx3
        .ProjectRevisions
        .Include(x => x.RevisionCalculationData)
        .ThenInclude(x => x!.EngineOperation)
        .FirstAsync();

    projectRevision.RevisionCalculationData = new RevisionCalculationData
    {
        EngineOperation = new FlexibleEngineOperationInformation
        {
            StartsPerYear = 11,
            OperatingHourRange = 2,
            RunTimePerYearAndEngine = 3,
            MonthsCalculationBegin = 4,
            StartsCalculationBegin = 5
        }
    };
    await ctx3.SaveChangesAsync();
}


Console.WriteLine("Done");