namespace HsManCommonLibrary.Attributes;

public enum TestStatus
{
    /// <summary>
    /// Indicates that the element has not been tested yet.
    /// </summary>
    NotTested,

    /// <summary>
    /// Indicates that the element is currently being tested.
    /// </summary>
    Testing,

    /// <summary>
    /// Indicates that the test for the element was successful.
    /// </summary>
    Passed,

    /// <summary>
    /// Indicates that the test for the element failed.
    /// </summary>
    Failed,

    /// <summary>
    /// Indicates that the test was blocked and could not be executed.
    /// </summary>
    Blocked,

    /// <summary>
    /// Indicates that the test was skipped and not executed.
    /// </summary>
    Skipped,

    /// <summary>
    /// Indicates that the test result is inconclusive.
    /// </summary>
    Inconclusive,

    /// <summary>
    /// Indicates that the test is not applicable to the marked element.
    /// </summary>
    NotApplicable,

    /// <summary>
    /// Indicates that the test result is under review.
    /// </summary>
    UnderReview
}