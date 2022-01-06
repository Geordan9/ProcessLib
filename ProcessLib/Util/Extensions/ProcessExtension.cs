using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ProcessLib.Util.Extensions
{
    [Flags]
    public enum ThreadAccess
    {
        TERMINATE = 0x0001,
        SUSPEND_RESUME = 0x0002,
        GET_CONTEXT = 0x0008,
        SET_CONTEXT = 0x0010,
        SET_INFORMATION = 0x0020,
        QUERY_INFORMATION = 0x0040,
        SET_THREAD_TOKEN = 0x0080,
        IMPERSONATE = 0x0100,
        DIRECT_IMPERSONATION = 0x0200
    }

    public static class ProcessExtension
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        [DllImport("kernel32.dll")]
        private static extern uint SuspendThread(IntPtr hThread);

        [DllImport("kernel32.dll")]
        private static extern int ResumeThread(IntPtr hThread);

        public static void Suspend(this Process process)
        {
            foreach (ProcessThread thread in process.Threads)
            {
                var pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint) thread.Id);
                if (pOpenThread == IntPtr.Zero) break;
                SuspendThread(pOpenThread);
            }
        }

        public static void Resume(this Process process)
        {
            foreach (ProcessThread thread in process.Threads)
            {
                var pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint) thread.Id);
                if (pOpenThread == IntPtr.Zero) break;
                ResumeThread(pOpenThread);
            }
        }

        public static bool IsSuspended(this Process process)
        {
            foreach (ProcessThread thread in process.Threads)
                if (thread.ThreadState == ThreadState.Wait)
                    if (thread.WaitReason == ThreadWaitReason.Suspended)
                        return true;
            return false;
        }

        public static bool IsWin64Emulator(this Process process)
        {
            if (Environment.OSVersion.Version.Major > 5
                || Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1)
                return NativeMethods.IsWow64Process(process.Handle, out var retVal) && retVal;

            return false;
        }

        private static string FindIndexedProcessName(int pid)
        {
            var processName = Process.GetProcessById(pid).ProcessName;
            var processesByName = Process.GetProcessesByName(processName);
            string processIndexdName = null;

            for (var index = 0; index < processesByName.Length; index++)
            {
                processIndexdName = index == 0 ? processName : processName + "#" + index;
                var processId = new PerformanceCounter("Process", "ID Process", processIndexdName);
                if ((int) processId.NextValue() == pid) return processIndexdName;
            }

            return processIndexdName;
        }

        private static Process FindPidFromIndexedProcessName(string indexedProcessName)
        {
            var parentId = new PerformanceCounter("Process", "Creating Process ID", indexedProcessName);
            return Process.GetProcessById((int) parentId.NextValue());
        }

        public static Process Parent(this Process process)
        {
            return FindPidFromIndexedProcessName(FindIndexedProcessName(process.Id));
        }
    }
}