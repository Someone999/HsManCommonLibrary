namespace HsManCommonLibrary.NestedValues;

public interface IEnumerableNestedValueStore : INestedValueStore, IEnumerable<INestedValueStore>
{
}