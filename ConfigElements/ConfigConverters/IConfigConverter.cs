namespace HsManCommonLibrary.ConfigElements.ConfigConverters;

public interface IConfigConverter
{
    object Convert(IConfigElement configElement);
}

public interface IConfigConverter<out T> : IConfigConverter
{
    new T Convert(IConfigElement configElement);
}