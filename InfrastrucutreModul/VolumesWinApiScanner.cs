

using Common.Interfaces;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;



namespace InfrastrucutreModul
{
    public class VolumesWinApiScanner : IVolumesWinApiScanner
    {
        public IEnumerable<VolumesWinApiResults> GetVolumes()
        {
            var volumes = new List<VolumesWinApiResults>();

            char[] volumeName = new char[256];
            var findHandle = NativeMethods.FindFirstVolume(volumeName, (uint)volumeName.Length);

            if (findHandle == IntPtr.Zero)
            {
                int error = Marshal.GetLastWin32Error();
                return volumes;
            }

            do
            {
                var volumeNameStr = (new string(volumeName)).Replace("\0", "");

                var volumesWinApiResults = new VolumesWinApiResults() { VolumeId = volumeNameStr };

                var volumeHandle = NativeMethods.CreateFile(volumeNameStr, 0, 0, IntPtr.Zero, 0, 0, IntPtr.Zero);
                if (volumeHandle == IntPtr.Zero)
                {
                    int error = Marshal.GetLastWin32Error();
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
                        volumesWinApiResults.DiskNumber = extent.DiskNumber;
                        volumesWinApiResults.StartingOffset = extent.StartingOffset;
                        volumesWinApiResults.ExtentLength = extent.ExtentLength;
                    }
                }
                else
                {
                    int error = Marshal.GetLastWin32Error();
                    //Console.WriteLine("DeviceIoControl error: " + error);// TODO: here is Error 6: Invalid Handle
                }

                volumes.Add(volumesWinApiResults);

                NativeMethods.CloseHandle(volumeHandle);

            } while (NativeMethods.FindNextVolume(findHandle, volumeName, (uint)volumeName.Length));

            NativeMethods.FindVolumeClose(findHandle);

            return volumes;
        }
    }
}
