using System.Runtime.InteropServices;

namespace CitnDev.System.IO
{    
    public static class File
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool DeleteFileW([MarshalAs(UnmanagedType.LPWStr)]string lpFileName);

        public static bool Delete(string fileName)
        {
            return DeleteFileW(fileName);
        }
    }
}
