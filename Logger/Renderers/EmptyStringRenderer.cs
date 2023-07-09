namespace CommonLibrary.Logger.Renderers;

public class EmptyStringRenderer : IStringRenderer
{
    public string Render(ColoredStringContainer container, bool supportColor)
    {
        return "";
    }
}