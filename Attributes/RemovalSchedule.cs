namespace HsManCommonLibrary.Attributes;

/// <summary>
/// Indicates the planned time frame for the removal of the marked element.
/// </summary>
public enum RemovalSchedule
{
    /// <summary>
    /// The removal schedule is pending.
    /// </summary>
    Pending, 
    
    /// <summary>
    /// Indicates that the element will be removed in the next commit.
    /// </summary>
    NextCommit, 

    /// <summary>
    /// Indicates that the element will be removed in the next version.
    /// </summary>
    NextVersion, 

    /// <summary>
    /// Indicates that the element will be removed in the short-term.
    /// </summary>
    ShortTerm,

    /// <summary>
    /// Indicates that the element will be removed in the long-term.
    /// </summary>
    LongTerm   
}