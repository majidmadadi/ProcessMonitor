using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMonitor.Console
{
    public class ProcessModel
    {
        public string ProcessId { get; set; } = null!;
        public DateTime StartTime { get; set; }
    }
}
