using System.Diagnostics;
using System.Drawing;

namespace ProcessLib.Core
{
    public class ProcessSnapshot
    {
        public ProcessSnapshot(Process process)
        {
            Process = process;
            try
            {
                Icon = Icon.ExtractAssociatedIcon(process.MainModule.FileName);
            }
            catch
            {
                Icon = null;
            }
        }

        public Process Process { get; set; }

        public Icon Icon { get; set; }
    }
}