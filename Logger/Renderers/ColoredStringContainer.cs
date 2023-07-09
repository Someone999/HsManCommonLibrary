using System.Collections;

namespace CommonLibrary.Logger.Renderers;

public class ColoredStringContainer : IEnumerable<ColoredString>
{
    private List<ColoredString> _coloredStrings = new();

    private ColoredString[] AddedString => _coloredStrings.ToArray();

    public void Append(string str, TextColor color)
    {
        _coloredStrings.Add(new ColoredString(str, color));
    }
    
    public void AppendLine(string str, TextColor color)
    {
        _coloredStrings.Add(new ColoredString(str + '\n', color));
    }

    public IEnumerator<ColoredString> GetEnumerator()
    {
        return _coloredStrings.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public string RenderWith(IStringRenderer renderer, bool supportColor) => renderer.Render(this, supportColor);
}