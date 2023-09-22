
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace Infrastructure;

internal class NativeMethods
{
    public const uint IOCTL_VOLUME_GET_VOLUME_DISK_EXTENTS = 0x00056000;

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
}

