namespace HsManCommonLibrary.Versioning;

public interface IVersionIncrementStrategy<TVersion>
{
    TVersion IncrementVersion(TVersion version);
}