using HsManCommonLibrary.ValueHolders;

namespace HsManCommonLibrary.NestedValues;

public interface INestedValueStoreAccessor
{
    ValueHolder<T> GetAsValueHolder<T>();
    ValueHolder<INestedValueStore> GetMemberAsValueHolder(string memberName);
    ValueHolder<T> GetMemberValueAsValueHolder<T>(string memberName);
    bool TryGetValue<T>(out T? value);
    bool TryGetMember(string name, out INestedValueStore? value);
    bool TryGetMemberValue<T>(string name, out T? value);
}