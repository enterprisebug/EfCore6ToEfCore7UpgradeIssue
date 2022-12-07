namespace EFCoreDatabase.Models;

public class RevisionCalculationData
{
    //public Guid Id { get; set; }
    public Guid ProjectRevisionId { get; set; }
    public virtual EngineOperationInformation? EngineOperation { get; set; }
}

public class ProjectRevision
{
    public Guid Id { get; set; }

    public virtual RevisionCalculationData? RevisionCalculationData { get; set; }
}