
using Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;


namespace Infrastructure
{
    internal class VolumesScanner2 : IVolumesScanner
    {

        const uint IOCTL_VOLUME_GET_VOLUME_DISK_EXTENTS = 0x560000;

        [StructLayout(LayoutKind.Sequential)]
        public struct VOLUME_DISK_EXTENTS
        {
            public uint NumberOfDiskExtents;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public DISK_EXTENT[] Extents;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DISK_EXTENT
        {
            public uint DiskNumber;
            public long StartingOffset;
            public long ExtentLength;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr FindFirstVolume([Out] char[] lpszVolumeName, uint cchBufferLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool FindNextVolume(IntPtr hFindVolume, [Out] char[] lpszVolumeName, uint cchBufferLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool FindVolumeClose(IntPtr hFindVolume);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool DeviceIoControl(IntPtr hDevice, uint dwIoControlCode, IntPtr lpInBuffer, uint nInBufferSize, ref VOLUME_DISK_EXTENTS lpOutBuffer, uint nOutBufferSize, out uint lpBytesReturned, IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hObject);

public bool IsUserAdministrator()
    {
        bool isAdmin;
        try
        {
            WindowsIdentity user = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(user);
            isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
        catch (UnauthorizedAccessException ex)
        {
            isAdmin = false;
        }
        catch (Exception ex)
        {
            isAdmin = false;
        }
        return isAdmin;
    }
    public StringCollection GetVolumes()
        {
            Console.WriteLine("ISADMIN " + IsUserAdministrator());
            char[] volumeName = new char[256];
            IntPtr findHandle = FindFirstVolume(volumeName, (uint)volumeName.Length);

            if (findHandle == IntPtr.Zero)
            {
                int error = Marshal.GetLastWin32Error();
                Console.WriteLine("FindFirstVolume error: " + error);
                return new StringCollection();
            }

            do
            {
                Console.WriteLine("Volume Name: " + new string(volumeName));

                IntPtr volumeHandle = CreateFile(new string(volumeName), 0, 0, IntPtr.Zero, 0, 0, IntPtr.Zero);
                if (volumeHandle == IntPtr.Zero)
                {
                    int error = Marshal.GetLastWin32Error();
                    Console.WriteLine("CreateFile error: " + error);
                    return new StringCollection();
                }

                VOLUME_DISK_EXTENTS volumeExtents = new VOLUME_DISK_EXTENTS();
                uint bytesReturned = 0;
                bool success = DeviceIoControl(volumeHandle, IOCTL_VOLUME_GET_VOLUME_DISK_EXTENTS, IntPtr.Zero, 0, ref volumeExtents, (uint)Marshal.SizeOf(volumeExtents), out bytesReturned, IntPtr.Zero);

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

                CloseHandle(volumeHandle);

            } while (FindNextVolume(findHandle, volumeName, (uint)volumeName.Length));

            FindVolumeClose(findHandle);

            return new StringCollection();
        }
    }
}
