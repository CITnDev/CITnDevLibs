using System;
using System.IO;
using System.Reflection;

namespace CitnDev.System.Data
{
    internal class AssemblyLoadingProxy : MarshalByRefObject
    {
        // Methods
        public Version LoadAssembly(Version poILVersion, FileInfo pofiAssembly, out Version outMscorlibVersion)
        {
            Version oVersionAssembly = null;
            byte[] bytArray = null;
            outMscorlibVersion = null;
            try
            {
                bytArray = File.ReadAllBytes(pofiAssembly.FullName);
                var oAssemblyReflectionOnly = Assembly.ReflectionOnlyLoad(bytArray);
                var oVersionMscorlib = new Version(oAssemblyReflectionOnly.ImageRuntimeVersion.Substring(1));
                if (oVersionMscorlib <= poILVersion)
                {
                    oVersionAssembly = oAssemblyReflectionOnly.GetName().Version;
                    outMscorlibVersion = oVersionMscorlib;
                }
            }
            catch{/* Prevent error during assembly loading */}
            finally
            {
                if (bytArray != null)
                    Array.Clear(bytArray, 0, bytArray.Length);
            }
            return oVersionAssembly;
        }
    }
}
