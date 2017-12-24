using System.Diagnostics;
namespace Lab5 {
    public static class StopWatchUtil {
        public static long ElapsedNanoSeconds(this Stopwatch watch) {
            return watch.ElapsedTicks * 1000000000 / Stopwatch.Frequency;
        }
    }
}
