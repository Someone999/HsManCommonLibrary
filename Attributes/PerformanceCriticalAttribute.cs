namespace HsManCommonLibrary.Attributes;

/// <summary>
/// Indicates that the marked element is performance-critical and requires special attention for optimization.
/// </summary>
/// <remarks>
/// This attribute should be used to identify code segments that are crucial for performance, 
/// and therefore may need further optimization or careful review to ensure efficiency. 
/// It serves as a reminder to developers to prioritize performance considerations in these areas.
/// </remarks>
[AttributeUsage(AttributeTargets.All)]
public class PerformanceCriticalAttribute : Attribute
{
    public PerformanceCriticalAttribute(string? reason = null)
    {
        Reason = reason;
    }

    public string? Reason { get; set; }
}