namespace HsManCommonLibrary.Attributes;

/// <summary>
/// Indicates that the marked element is intended for internal use only within the project or assembly.
/// </summary>
/// <remarks>
/// This attribute should be applied to elements that are not meant to be exposed or used outside the intended scope. 
/// It serves as a reminder that the element is part of the internal implementation and might be subject to change 
/// or removal in future releases without notice. Ensure that such elements are not relied upon by external consumers.
/// </remarks>
[AttributeUsage(AttributeTargets.All)]
public class InternalUseOnlyAttribute : Attribute
{
}