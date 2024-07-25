namespace HsManCommonLibrary.Attributes;

/// <summary>
/// Indicates that the marked element will be removed in a future version.
/// </summary>
/// <remarks>
/// Elements marked with this attribute are planned for removal in future releases. 
/// If you are using such elements, consider transitioning to alternative solutions 
/// to avoid potential risks and maintain compatibility with future versions.
/// </remarks>
[AttributeUsage(AttributeTargets.All)]
public class WillBeRemovedAttribute : Attribute
{
    public WillBeRemovedAttribute(string message, RemovalSchedule removalSchedule)
    {
        Message = message;
        RemovalSchedule = removalSchedule;
    }
    
    public WillBeRemovedAttribute(RemovalSchedule removalSchedule)
    {
        RemovalSchedule = removalSchedule;
    }
    
    public WillBeRemovedAttribute()
    {
        RemovalSchedule = RemovalSchedule.Pending;
    }

    public string? Message { get; set; }
    public RemovalSchedule RemovalSchedule { get; }
}