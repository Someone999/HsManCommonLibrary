namespace HsManCommonLibrary.Timing;

public interface IClock
{ 
    TimeSpan ElapsedTime { get; }
    double Rate { get; }
}