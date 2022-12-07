using EFCoreDatabase.Contexts;
using Microsoft.EntityFrameworkCore;

await using (var ctx = new MyContext())
{
    await ctx.Database.EnsureDeletedAsync();
    await ctx.Database.EnsureCreatedAsync();
}

await using (var ctx2 = new MyContext())
{
    var projectRevsision = new MainEntity
    {
        ChildEntity = new ChildEntity
        {
            EntityWithInheritance = new EntityWithInheritanceOne
            {
                MyNumber = 1
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
        .Include(x => x.ChildEntity)
        .ThenInclude(x => x!.EntityWithInheritance)
        .FirstAsync();

    projectRevision.ChildEntity = new ChildEntity
    {
        EntityWithInheritance = new EntityWithInheritanceTwo
        {
            MyNumber = 11,
            MyOtherNumber = 5
        }
    };
    await ctx3.SaveChangesAsync();
}


Console.WriteLine("Done");