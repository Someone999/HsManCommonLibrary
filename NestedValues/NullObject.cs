namespace HsManCommonLibrary.NestedValues;

public class NullObject
{
    private NullObject()
    {
    }

    public override bool Equals(object? obj) => obj == null;

    public override int GetHashCode() => 0;

    public override string ToString() => "";

    public static NullObject? Value { get; } = new NullObject();
}