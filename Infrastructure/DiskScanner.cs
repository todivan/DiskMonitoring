
using Interfaces;
using System;

namespace Infrastructure;

//internal class DiskScanner : IDiskScanner
//{
//    public IEnumerable<string> GetDisks()
//    {
//        ManagementObjectSearcher ms = new ManagementObjectSearcher("Select * from Win32_Volume");
//        foreach (ManagementObject mo in ms.Get())
//        {
//            var guid = mo["DeviceID"].ToString();

//            if (guid == myGuid)
//                return mo["DriveLetter"];
//        }
//    }
//}

