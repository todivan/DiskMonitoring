using Interfaces;
using Microsoft.Win32.SafeHandles;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;

namespace Infrastructure;

public class VolumesScanner : IVolumesScanner
{
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern FindVolumeSafeHandle FindFirstVolume([Out] StringBuilder lpszVolumeName, uint cchBufferLength);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool FindNextVolume(FindVolumeSafeHandle hFindVolume, [Out] StringBuilder lpszVolumeName, uint cchBufferLength);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool FindVolumeClose(IntPtr hFindVolume);

    public class FindVolumeSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        private FindVolumeSafeHandle()
        : base(true)
        {
        }

        public FindVolumeSafeHandle(IntPtr preexistingHandle, bool ownsHandle)
        : base(ownsHandle)
        {
            SetHandle(preexistingHandle);
        }

        protected override bool ReleaseHandle()
        {
            return FindVolumeClose(handle);
        }
    }

    public StringCollection GetVolumes()
    {
        const uint bufferLength = 1024;
        StringBuilder volume = new StringBuilder((int)bufferLength, (int)bufferLength);
        StringCollection ret = new StringCollection();

        using (FindVolumeSafeHandle volumeHandle = FindFirstVolume(volume, bufferLength))
        {
            if (volumeHandle.IsInvalid)
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
            }

            do
            {
                ret.Add(volume.ToString());
                GetDevices(volume.ToString());
            } while (FindNextVolume(volumeHandle, volume, bufferLength));

            return ret;
        }
    }
    
    private void GetDevices(string volume)
    {
        // Open the volume handle using CreateFile()
        string vol = @"\\?\Volume{ead7ee26-b0bb-4c0c-acee-b4c78fe88426}";
        SafeFileHandle sfh = NativeMethods.CreateFile(@"\\.\D:\tmp.txt", FileAccess.Read, FileShare.ReadWrite | FileShare.Delete, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);



        //SafeFileHandle sfh = NativeMethods.CreateFile(vol, NativeMethods.GENERIC_READ, 0, IntPtr.Zero, NativeMethods.OPEN_EXISTING,
        //    NativeMethods.FILE_ATTRIBUTE_NORMAL, new SafeFileHandle(IntPtr.Zero, true));

        if (sfh.IsInvalid)
        {
            throw new Win32Exception();
        }

        // Prepare to obtain disk extents.
        // NOTE: This code assumes you only have one disk!
        NativeMethods.VOLUME_DISK_EXTENTS vde = new NativeMethods.VOLUME_DISK_EXTENTS();
        UInt32 outBufferSize = (UInt32)Marshal.SizeOf(vde);
        IntPtr outBuffer = Marshal.AllocHGlobal((int)outBufferSize);
        UInt32 bytesReturned = 0;
        if (NativeMethods.DeviceIoControl(sfh,
                                          NativeMethods.IOCTL_VOLUME_GET_VOLUME_DISK_EXTENTS,
                                          IntPtr.Zero,
                                          0,
                                          outBuffer,
                                          outBufferSize,
                                          out bytesReturned,
                                          IntPtr.Zero))
        {
            // The call succeeded, so marshal the data back to a
            // form usable from managed code.
            Marshal.PtrToStructure(outBuffer, vde);

            // Do something with vde.Extents here...e.g.
            Console.WriteLine("DiskNumber: {0}\nStartingOffset: {1}\nExtentLength: {2}",
                              vde.Extents.DiskNumber,
                              vde.Extents.StartingOffset,
                              vde.Extents.ExtentLength);
        }
        Marshal.FreeHGlobal(outBuffer);
    }

}