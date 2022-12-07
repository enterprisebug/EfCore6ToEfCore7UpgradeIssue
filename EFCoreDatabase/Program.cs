using EFCoreDatabase.Contexts;
using Microsoft.EntityFrameworkCore;

await using (var ctx = new MyContext())
{
    await ctx.Database.EnsureDeletedAsync();
    await ctx.Database.EnsureCreatedAsync();
}

await using (var ctx2 = new MyContext())
{
    var mainEntity = new MainEntity
    {
        ChildEntity = new ChildEntity
        {
            EntityWithInheritance = new EntityWithInheritanceOne
            {
                MyNumber = 1
            }
        }
    };

    await ctx2.MainEntities.AddAsync(mainEntity);
    await ctx2.SaveChangesAsync();
}

await using (var ctx3 = new MyContext())
{
    var mainEntity = await ctx3
        .MainEntities
        .Include(x => x.ChildEntity)
        .ThenInclude(x => x!.EntityWithInheritance)
        .FirstAsync();

    mainEntity.ChildEntity = new ChildEntity
    {
        EntityWithInheritance = new EntityWithInheritanceTwo
        {
            MyNumber = 11,
            MyOtherNumber = 5
        }
    };
    await ctx3.SaveChangesAsync();
}

await using (var ctx4 = new MyContext())
{
    var mainEntity = await ctx4
        .MainEntities
        .Include(x => x.ChildEntity)
        .ThenInclude(x => x!.EntityWithInheritance)
        .FirstAsync();

    if (mainEntity.ChildEntity.EntityWithInheritance.GetType() != typeof(EntityWithInheritanceTwo))
        throw new Exception("Invalid behavior");
}