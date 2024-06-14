namespace HsManCommonLibrary.CommandLine.Matchers;

public class DuplicateOptionHandlingResult
{
    public DuplicateOptionHandlingResult(bool continueExecute, string? errorMassage = null)
    {
        ContinueExecute = continueExecute;
        ErrorMassage = errorMassage;
    }
    public bool ContinueExecute { get; }
    public string? ErrorMassage { get; }
}