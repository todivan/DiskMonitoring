
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace Infrastructure;

internal class NativeMethods
{
    internal const UInt32 IOCTL_VOLUME_GET_VOLUME_DISK_EXTENTS = 0x00560000;

    [StructLayout(LayoutKind.Sequential)]
    internal class DISK_EXTENT
    {
        public UInt32 DiskNumber;
        public Int64 StartingOffset;
        public Int64 ExtentLength;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal class VOLUME_DISK_EXTENTS
    {
        public UInt32 NumberOfDiskExtents;
        public DISK_EXTENT Extents;
    }

    [DllImport("kernel32", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool DeviceIoControl(SafeFileHandle hDevice,
                                                UInt32 ioControlCode,
                                                IntPtr inBuffer,
                                                UInt32 inBufferSize,
                                                IntPtr outBuffer,
                                                UInt32 outBufferSize,
                                                out UInt32 bytesReturned,
                                                IntPtr overlapped);

    //[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    //internal static extern SafeFileHandle CreateFile(string lpFileName,
    //                int dwDesiredAccess,
    //                int dwShareMode,
    //                IntPtr lpSecurityAttributes,
    //                uint dwCreationDisposition,
    //                uint dwFlagsAndAttributes,
    //                SafeFileHandle hTemplateFile);

    internal const int GENERIC_READ = unchecked((int)0x80000000);
    internal const uint GENERIC_ALL = unchecked((uint)0x10000000);
    internal const uint OPEN_EXISTING = 3;
    internal const uint FILE_ATTRIBUTE_NORMAL = 0x80;


    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern SafeFileHandle CreateFile(
 [MarshalAs(UnmanagedType.LPTStr)] string filename,
 [MarshalAs(UnmanagedType.U4)] FileAccess access,
 [MarshalAs(UnmanagedType.U4)] FileShare share,
 IntPtr securityAttributes, // optional SECURITY_ATTRIBUTES struct or IntPtr.Zero
 [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
 [MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes,
 IntPtr templateFile);
}

