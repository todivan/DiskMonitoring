using System;
using System.Collections.Generic;
using System.Management;
using System.Text;

namespace InfrastrucutreModul
{
    public abstract class WmiScannerBase
    {
        public ManagementObjectCollection ExecuteCommand(string command)
        {
            ManagementObjectSearcher ms = new ManagementObjectSearcher(command);
            return ms.Get();
        }
    }
}
