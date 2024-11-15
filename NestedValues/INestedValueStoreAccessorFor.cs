using HsManCommonLibrary.ValueHolders;

namespace HsManCommonLibrary.NestedValues;

public interface INestedValueStoreAccessorFor
{
    ValueHolder<T> GetAsValueHolder<T>(INestedValueStore store);
    ValueHolder<INestedValueStore> GetMemberAsValueHolder(string memberName, INestedValueStore store);
    ValueHolder<T> GetMemberValueAsValueHolder<T>(string memberName, INestedValueStore store);
    bool TryGetValue<T>(out T? value, INestedValueStore store);
    bool TryGetMember(string name, out INestedValueStore? value, INestedValueStore store);
    bool TryGetMemberValue<T>(string name, out T? value, INestedValueStore store);
}