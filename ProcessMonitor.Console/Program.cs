
using ProcessMonitor.Console;

if (args.Length < 3)
{
    Console.WriteLine("Incorrect parameters. \nUsage: ProcessMonitor.Console.exe [process_name] [threshold_in_minutes] [interval_in_minutes]\n");
    Environment.Exit(1);
}

var processName = args[0];
var threshold = int.Parse(args[1]);
var interval = int.Parse(args[2]);

var timer = new Timer(CallBack, null, 0, interval * 60 * 1000);

Console.WriteLine($"Monitoring for process {processName}. See log.txt for logs. Press q to quit.");
while (true)
{
    var key = Console.ReadKey().KeyChar;
    if (key == 'q') break;
}

void CallBack(object? o)
{
    var prosesses = Win32ProcessManager.GetProcesses(processName);
    if (prosesses.Count > 0)
    {
        foreach (var p in prosesses)
        {
            if (p.StartTime.AddMinutes(threshold) <= DateTime.Now)
            {
                Win32ProcessManager.TerminateProcess(p.ProcessId);
                Console.WriteLine($"Killed process {processName}:{p.ProcessId} ...");
            }
        }
    }
}
