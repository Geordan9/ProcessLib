using System;

namespace ProcessLib.Utils
{
    public static class Check64bit
    {
        public static bool is64bitProcess = Environment.Is64BitProcess;

        public static bool is64bitOS = Environment.Is64BitOperatingSystem;
    }
}