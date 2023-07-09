namespace CommonLibrary.Utils;

public static class TimeUtils
{
    public static long ToTimestamp(DateTime dateTime)
    {
        DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0);
        if (dateTime < unixEpoch)
        {
            throw new ArgumentException("Specified time must larger then UnixEpoch");
        }

        return (long)(dateTime - unixEpoch).TotalSeconds;
    }

    public static DateTime FromTimestamp(long timestamp)
    {
        DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0);
        return unixEpoch.AddSeconds(timestamp);
    }
    
    public static long ToTimestampMillisecond(DateTime dateTime)
    {
        DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0);
        if (dateTime < unixEpoch)
        {
            throw new ArgumentException("Specified time must larger then UnixEpoch");
        }

        return (long)(dateTime - unixEpoch).TotalMilliseconds;
    }

    public static DateTime FromTimestampMillisecond(long timestamp)
    {
        DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0);
        return unixEpoch.AddMilliseconds(timestamp);
    }

    public static long GetCurrentTimestamp() => ToTimestamp(DateTime.UtcNow);
    public static long GetCurrentTimestampMillisecond() => ToTimestampMillisecond(DateTime.UtcNow);

}