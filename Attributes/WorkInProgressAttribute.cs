namespace HsManCommonLibrary.Attributes;

/// <summary>
/// Indicates that the marked element is developing.
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public class WorkInProgressAttribute : Attribute
{
    public WorkInProgressAttribute(string message, double progress)
    {
        Message = message;
        Progress = progress;
    }
    
    public WorkInProgressAttribute(string message)
    {
        Message = message;
        Progress = 0;
    }
    
    public WorkInProgressAttribute()
    {
    }

    public string? Message { get; }
    public double Progress { get; }
}