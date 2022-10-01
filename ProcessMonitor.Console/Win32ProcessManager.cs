using System.Management;
using System.Text.RegularExpressions;

namespace ProcessMonitor.Console
{
    public static class Win32ProcessManager
    {
        public static List<ProcessModel> GetProcesses(string processName)
        {
            try
            {
                var searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    $"SELECT * FROM Win32_Process WHERE Name = '{processName}'");

                var processes = new List<ProcessModel>();
                foreach (var queryObj in searcher.Get())
                {
                    processes.Add(new ProcessModel
                    {
                        ProcessId = queryObj["ProcessId"].ToString() ?? "",
                        StartTime = ParseDateTime(queryObj["CreationDate"].ToString() ?? "")
                    });
                }
                return processes;
            }
            catch (ManagementException e)
            {
                throw new Exception("An error occurred while querying for WMI data: " + e.Message);
            }
        }

        public static void TerminateProcess(string processId)
        {
            try
            {
                var classInstance =
                    new ManagementObject("root\\CIMV2",
                    $"Win32_Process.Handle='{processId}'",
                    null);

                // Obtain in-parameters for the method
                var inParams =
                    classInstance.GetMethodParameters("Terminate");

                // Add the input parameters.

                // Execute the method and obtain the return values.
                ManagementBaseObject outParams =
                    classInstance.InvokeMethod("Terminate", inParams, null);

                // List outParams
                File.AppendAllLines(".\\log.txt",
                    new[] { $"{DateTime.Now} ProcessId {processId} terminated successfully." });
            }
            catch (ManagementException err)
            {
                File.AppendAllLines(".\\log.txt",
                        new[] { $"{DateTime.Now} An error occurred while trying to execute the WMI method: {err.Message}" });
            }
        }

        public static DateTime ParseDateTime(string str)
        {
            var match = Regex.Match(str, @"(\d{4})(\d{2})(\d{2})(\d{2})(\d{2})(\d{2})\..*");
            var dateStr = $"{match.Groups[1].Value}-{match.Groups[2].Value}-{match.Groups[3].Value} {match.Groups[4].Value}:{match.Groups[5].Value}:{match.Groups[6].Value}";
            return DateTime.Parse(dateStr);
        }
    }
}
