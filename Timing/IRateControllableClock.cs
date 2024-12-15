namespace HsManCommonLibrary.Timing;

public interface IRateControllableClock : IControllableClock
{
    new double Rate { get; set; }
}