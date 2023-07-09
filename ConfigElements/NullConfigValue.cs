namespace CommonLibrary.ConfigElements;

public class NullConfigValue
{
    private NullConfigValue()
    {
    }

    public override bool Equals(object? obj) => obj == null;

    public override int GetHashCode() => 0;

    public override string ToString() => "";

    public static NullConfigValue Value { get; } = new NullConfigValue();
}