using HsManCommonLibrary.ConfigElements.ConfigConverters;

namespace HsManCommonLibrary.ConfigElements;

public interface IConfigElement
{
    object GetValue();
    void SetValue(string key, IConfigElement val);
    IConfigElement this[string key] { get; set; }
    object? Convert(Type type);
    T Convert<T>();
    object ConvertWith(IConfigConverter converter);
    T ConvertWith<T>(IConfigConverter<T> converter);
    bool IsNull(string key);
}