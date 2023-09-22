

using InfrastrucutreModul.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;


namespace InfrastrucutreModul
{
    public class VolumesWinApiScanner
    {
        public List<VolumesWinApiResults> GetVolumes()
        {
            var volumes = new List<VolumesWinApiResults>();

            char[] volumeName = new char[256];
            var findHandle = NativeMethods.FindFirstVolume(volumeName, (uint)volumeName.Length);

            if (findHandle == IntPtr.Zero)
            {
                int error = Marshal.GetLastWin32Error();
                Console.WriteLine("FindFirstVolume error: " + error);
                return volumes;
            }

            do
            {
                var volumeNameStr = new string(volumeName);
                Console.WriteLine("Volume Name: " + volumeNameStr);

                volumes.Add(new VolumesWinApiResults() { VolumeId = volumeNameStr.Replace("\0", "") });

                var volumeHandle = NativeMethods.CreateFile(new string(volumeName), 0, 0, IntPtr.Zero, 0, 0, IntPtr.Zero);
                if (volumeHandle == IntPtr.Zero)
                {
                    int error = Marshal.GetLastWin32Error();
                    Console.WriteLine("CreateFile error: " + error);
                    return volumes;
                }

                NativeMethods.VOLUME_DISK_EXTENTS volumeExtents = new NativeMethods.VOLUME_DISK_EXTENTS();
                bool success = NativeMethods.DeviceIoControl(volumeHandle, NativeMethods.IOCTL_VOLUME_GET_VOLUME_DISK_EXTENTS, IntPtr.Zero, 0, 
                    ref volumeExtents, (uint)Marshal.SizeOf(volumeExtents), out uint bytesReturned, IntPtr.Zero);


                if (success)
                {
                    Console.WriteLine("Number of Disk Extents: " + volumeExtents.NumberOfDiskExtents);
                    foreach (var extent in volumeExtents.Extents)
                    {
                        Console.WriteLine("Disk Number: " + extent.DiskNumber);
                        Console.WriteLine("Starting Offset: " + extent.StartingOffset);
                        Console.WriteLine("Extent Length: " + extent.ExtentLength);
                    }
                }
                else
                {
                    int error = Marshal.GetLastWin32Error();
                    Console.WriteLine("DeviceIoControl error: " + error);
                }

                NativeMethods.CloseHandle(volumeHandle);

            } while (NativeMethods.FindNextVolume(findHandle, volumeName, (uint)volumeName.Length));

            NativeMethods.FindVolumeClose(findHandle);

            return volumes;
        }
    }
}
