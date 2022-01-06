using System;

namespace ProcessLib.Util
{
    public static class Check64bit
    {
        public static bool Is64bitProcess = Environment.Is64BitProcess;

        public static bool Is64bitOS = Environment.Is64BitOperatingSystem;
    }
}