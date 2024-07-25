namespace HsManCommonLibrary.Attributes;

/// <summary>
/// Indicates the testing status of the marked element.
/// </summary>
/// <remarks>
/// Use this attribute to indicate the testing status of code elements. The <see cref="Status"/> property allows
/// you to specify whether the element has not been tested, is currently being tested, or has passed or failed testing.
/// </remarks>
[AttributeUsage(AttributeTargets.All)]
public class TestStatusAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the testing status of the element.
    /// </summary>
    public TestStatus Status { get; }

    public string? AdditionalInfo { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestStatusAttribute"/> class with a specified testing status.
    /// </summary>
    /// <param name="status">The testing status of the element.</param>
    /// <param name="additionalInfo">Additional information for the test status.</param>
    public TestStatusAttribute(TestStatus status, string? additionalInfo)
    {
        Status = status;
        AdditionalInfo = additionalInfo;
    }
}