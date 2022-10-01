using NUnit.Framework;
using ProcessMonitor.Console;
using System.Diagnostics;

namespace ProcessManager.Tests
{
    [TestFixture]
    public class ProcessManagerTerminateTests
    {
        [Test]
        public void ShouldTerminateOneProcess()
        {
            var appName = "notepad.exe";
            var p = Process.Start(appName);
            var processes = Win32ProcessManager.GetProcesses(appName);
            //make sure it finds the process
            Assert.That(processes, Has
                .Exactly(1).Property("ProcessId")
                .EqualTo(p.Id.ToString()));
            Win32ProcessManager.TerminateProcess(p.Id.ToString());
            processes = Win32ProcessManager.GetProcesses(appName);
            //make sure it finds the process
            Assert.That(processes, Has
                .Exactly(0).Property("ProcessId")
                .EqualTo(p.Id.ToString()));
        }
    }
}
