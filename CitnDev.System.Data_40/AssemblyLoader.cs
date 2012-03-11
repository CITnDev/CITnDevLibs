using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Policy;

namespace CitnDev.System.Data
{
    internal class AssemblyLoader
    {
        public static FileInfo GetBestAssembly(string pstrAssemblyName)
        {
            FileInfo ofiBestOracleAssembly = null;
            Version oBestOracleVersion = null;
            Version oBestVersionOracleMscorlib = null;
            var oAssemblyArray = AppDomain.CurrentDomain.GetAssemblies();
            var oVersionMscorlib = new Version(oAssemblyArray[0].ImageRuntimeVersion.Substring(1));
            if (oVersionMscorlib.Major == 1) { throw new InvalidProgramException("Library no compliant with CLR 1.x"); }
            Debug.WriteLine("Running assembly mscorlib version : " + oVersionMscorlib);
            Array.Clear(oAssemblyArray, 0, oAssemblyArray.Length);
// ReSharper disable AssignNullToNotNullAttribute
            var anyCPUXp = 
                new DirectoryInfo(Path.Combine(Environment.GetEnvironmentVariable("windir"), @"assembly\GAC\" + pstrAssemblyName));
            var anyCPUW7 =
                new DirectoryInfo(Path.Combine(Environment.GetEnvironmentVariable("windir"), @"assembly\GAC_MSIL\" + pstrAssemblyName));
            var x86 =
                new DirectoryInfo(Path.Combine(Environment.GetEnvironmentVariable("windir"), @"assembly\GAC_32\" + pstrAssemblyName));
            var x64 =
                new DirectoryInfo(Path.Combine(Environment.GetEnvironmentVariable("windir"), @"assembly\GAC_64\" + pstrAssemblyName))
                ;
            var oGacList = new List<DirectoryInfo>(new[] {anyCPUW7, anyCPUXp}); 
            
            switch(IntPtr.Size)
            {
                case 4:
                    oGacList.Add(x86);
                    break;
                case 8:
                    oGacList.Add(x64);
                    break;
                default:
                    throw new PlatformNotSupportedException();
            }
            
// ReSharper restore AssignNullToNotNullAttribute
            try
            {
                foreach (var odiGacOracle in oGacList)
                {
                    if ((odiGacOracle != null) && odiGacOracle.Exists)
                    {
                        //AdvancedTrace.TraceInformation("Check directory : " + odiGacOracle.FullName);
                        foreach (DirectoryInfo dirVersion in odiGacOracle.GetDirectories())
                        {
                            //AdvancedTrace.TraceInformation("Check directory : " + dirVersion.FullName);
                            var ofiAssembly = new FileInfo(Path.Combine(dirVersion.FullName, pstrAssemblyName + ".dll"));
                            if (ofiAssembly.Exists)
                            {
                                //AdvancedTrace.TraceInformation("Load assembly for checking : " + ofiAssembly.FullName);
                                Evidence evidence = new Evidence(AppDomain.CurrentDomain.Evidence);
                                AppDomainSetup setup = AppDomain.CurrentDomain.SetupInformation;
                                AppDomain oAppDomain = AppDomain.CreateDomain("_ASSEMBLY_LOADER_", evidence, setup);
                                Version oVersionOracleMscorlib;
// ReSharper disable PossibleNullReferenceException
                                Version oOracleVersion = (oAppDomain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().GetName().FullName, "CitnDev.System.Data.AssemblyLoadingProxy") as AssemblyLoadingProxy).LoadAssembly(oVersionMscorlib, ofiAssembly, out oVersionOracleMscorlib);
// ReSharper restore PossibleNullReferenceException
#if DEBUG
                                if (oVersionOracleMscorlib != null) { Debug.WriteLine("\tmscorlib version : " + oVersionOracleMscorlib); }
                                else { Debug.WriteLine("\tmscorlib version : none"); }

                                if (oBestOracleVersion == null) { Debug.WriteLine("\tBest version : none"); }
                                else { Debug.WriteLine("\tBest version : " + oBestOracleVersion); }

                                if (oOracleVersion != null) { Debug.WriteLine("\tCurrent version : " + oOracleVersion); }
                                else { Debug.WriteLine("\tCurrent version : none"); }
#endif
                                if (oVersionOracleMscorlib != null)
                                {
                                    if (oBestVersionOracleMscorlib == null)
                                    {
                                        Debug.WriteLine("\t===>First version found");
                                        oBestOracleVersion = oOracleVersion;
                                        oBestVersionOracleMscorlib = oVersionOracleMscorlib;
                                        ofiBestOracleAssembly = ofiAssembly;
                                    }
                                    else if (oBestVersionOracleMscorlib < oVersionOracleMscorlib)
                                    {
                                        Debug.WriteLine("\t===>Better MSIL found");
                                        oBestOracleVersion = oOracleVersion;
                                        oBestVersionOracleMscorlib = oVersionOracleMscorlib;
                                        ofiBestOracleAssembly = ofiAssembly;
                                    }
                                    else if ((oBestVersionOracleMscorlib == oVersionOracleMscorlib) && (oBestOracleVersion < oOracleVersion))
                                    {
                                        Debug.WriteLine("\t===>Better version for the same MSIL found");
                                        oBestOracleVersion = oOracleVersion;
                                        oBestVersionOracleMscorlib = oVersionOracleMscorlib;
                                        ofiBestOracleAssembly = ofiAssembly;
                                    }
                                }
                                AppDomain.Unload(oAppDomain);
                            }
                        }
                    }
                }
            }
            catch (Exception oException)
            {
                Debug.WriteLine("Error during searching best oracle provider " + oException);
            }
            return ofiBestOracleAssembly;
        }
    }
}