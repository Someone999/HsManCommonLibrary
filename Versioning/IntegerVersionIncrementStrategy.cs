namespace HsManCommonLibrary.Versioning;

public class IntegerVersionIncrementStrategy : IVersionIncrementStrategy<int>
{
    public int IncrementVersion(int version) => version + 1;

}