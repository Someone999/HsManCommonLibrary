namespace HsManCommonLibrary.Attributes;

/// <summary>
/// Indicates that the marked element has deprecated and will be removed in the future. <br />
/// This can be used combine with <seealso cref="WillBeRemovedAttribute"/> 
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public class DeprecatedAttribute : Attribute
{
    public DeprecatedAttribute(string? message, RemovalSchedule removalSchedule)
    {
        Message = message;
        RemovalSchedule = removalSchedule;
    }
    
    public DeprecatedAttribute(RemovalSchedule removalSchedule)
    {
        RemovalSchedule = removalSchedule;
    }
    
    public DeprecatedAttribute(string? message = null)
    {
        Message = message;
        RemovalSchedule = RemovalSchedule.Pending;
    }

    public string? Message { get; }
    public RemovalSchedule RemovalSchedule { get; }
}