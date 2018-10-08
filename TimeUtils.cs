using System;
using System.Collections.Generic;

public class TimeUtils  {
    public static long getCurrentMs()
    {
        return System.DateTime.Now.Ticks / 10000L;
    }
    public static long getRemainingTimeMs(long startTimeMs, long waitTimeMs)
    {
        return startTimeMs + waitTimeMs - getCurrentMs();
    }
    public static bool isOverTime(long startTimeMs, long waitTimeMs)
    {
        return getCurrentMs() > startTimeMs + waitTimeMs;
    }

    public static string ConvertMillisecondsToDisplayTime(long milliSeconds)
    {
        if(milliSeconds >= 0)
        {
            TimeSpan t = TimeSpan.FromMilliseconds(milliSeconds);
            string time = string.Format("{0:D2}:{1:D2}:{2:D2}",
                                    t.Hours,
                                    t.Minutes,
                                    t.Seconds);
            return time;
        }
        return "";
    }
    public static string ConvertMillisecondsToShort(long milliSeconds)
    {
        if (milliSeconds >= 0)
        {
            TimeSpan t = TimeSpan.FromMilliseconds(milliSeconds);
            List<string> time = new List<string>();
            if (t.Days > 0)
                time.Add(t.Days + "d");
            if (t.Hours > 0)
                time.Add(t.Hours + "h");
            if(t.Seconds > 0)
                time.Add(t.Seconds + "s");


            return string.Join(" ",time.ToArray());
        }
        return "";
    }
}
