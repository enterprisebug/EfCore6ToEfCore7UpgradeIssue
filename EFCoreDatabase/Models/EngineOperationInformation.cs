namespace EFCoreDatabase.Models;

public static class EngineOperationInformationConfig
{
    public const string ContinuousEngineOperationInformation = "ContinuousEngineOperationInformation";
    public const string FlexibleEngineOperationInformation = "FlexibleEngineOperationInformation";
    public const string DiscriminatorColumnName = "Discriminator";
}

public abstract class EngineOperationInformation
{
    //public Guid Id { get; set; }
    public Guid RevisionCalculationDataId { get; set; }
    public int StartsPerYear { get; set; }
    public int RunTimePerYearAndEngine { get; set; }
    public int OperatingHourRange { get; set; }
}

public class ContinuousEngineOperationInformation : EngineOperationInformation
{
}

public class FlexibleEngineOperationInformation : EngineOperationInformation
{
    public int StartsCalculationBegin { get; set; }
    public int MonthsCalculationBegin { get; set; }
}