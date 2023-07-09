namespace CommonLibrary.Logger.Renderers;

public interface IStringRenderer
{
    string Render(ColoredStringContainer container, bool supportColor);
}