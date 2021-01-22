using System.Diagnostics;
using System.Drawing;

namespace ProcessLib.Models
{
    public class ProcessSnapshot
    {
        public Process Process { get; set; }

        public Icon Icon { get; set; }

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
    }
}