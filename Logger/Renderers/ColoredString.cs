namespace CommonLibrary.Logger.Renderers;

public class ColoredString
{
    public ColoredString(string text,TextColor color)
    {
        Text = text;
        Color = color;
    }

    public TextColor Color { get; set; }
    public string Text { get; set; }
}