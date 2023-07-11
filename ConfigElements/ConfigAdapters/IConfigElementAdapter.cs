namespace HsManCommonLibrary.ConfigElements.ConfigAdapters;

public interface IConfigElementAdapter
{
    IConfigElement ToConfigElement(object? obj);
    bool CanConvert(Type t);
}