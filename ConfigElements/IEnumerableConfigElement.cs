namespace CommonLibrary.ConfigElements;

public interface IEnumerableConfigElement : IConfigElement, IEnumerable<IConfigElement>
{
}