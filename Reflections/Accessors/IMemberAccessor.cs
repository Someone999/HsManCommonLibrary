namespace HsManCommonLibrary.Reflections.Accessors;

public interface IMemberAccessor
{
    object? GetValue(object? instance, params object?[]? args);
    void SetValue(object? instance, object? value, params object?[]? args);
}