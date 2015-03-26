using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;

namespace Helper
{
	[SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
	[SuppressMessage("ReSharper", "UnusedMember.Local")]
	public static class NativeMethods
    {
		public const String Kernel32 = "kernel32.dll";
	    private const String Winmm = "winmm.dll";
	    private const String User32 = "user32.dll";
	    private const String Urlmon = "urlmon.dll";

        const Int32 UrlmonOptionUserAgent = 0x10000001;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Global")]
        public struct StartupInfo
        {
            public Int32 cb;
            public String lpReserved;
            public String lpDesktop;
            public String lpTitle;
            public Int32 dwX;
            public Int32 dwY;
            public Int32 dwXSize;
            public Int32 dwYSize;
            public Int32 dwXCountChars;
            public Int32 dwYCountChars;
            public Int32 dwFillAttribute;
            public Int32 dwFlags;
            public Int16 wShowWindow;
            public Int16 cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        [StructLayout(LayoutKind.Sequential)]
		[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
		[SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Global")]
        public struct ProcessInformation
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public Int32 dwProcessId;
            public Int32 dwThreadId;
        }

        [StructLayout(LayoutKind.Sequential)]
		[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
		[SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Global")]
        public struct SecurityAttributes
        {
            public Int32 nLength;
            public IntPtr lpSecurityDescriptor;
            public Int32 bInheritHandle;
        }

        [Flags]
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public enum AllocationType
        {
            Commit = 0x1000,
            Reserve = 0x2000,
            Decommit = 0x4000,
            Release = 0x8000,
            Reset = 0x80000,
            Physical = 0x400000,
            TopDown = 0x100000,
            WriteWatch = 0x200000,
            LargePages = 0x20000000
        }

        [Flags]
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public enum MemoryProtection
        {
            Execute = 0x10,
            ExecuteRead = 0x20,
            ExecuteReadWrite = 0x40,
            ExecuteWriteCopy = 0x80,
            NoAccess = 0x01,
            ReadOnly = 0x02,
            ReadWrite = 0x04,
            WriteCopy = 0x08,
            GuardModifierflag = 0x100,
            NoCacheModifierflag = 0x200,
            WriteCombineModifierflag = 0x400
        }

        [Flags]
        public enum ProcessCreationFlags : uint
        {
            ZeroFlag = 0x00000000,
            CreateBreakawayFromJob = 0x01000000,
            CreateDefaultErrorMode = 0x04000000,
            CreateNewConsole = 0x00000010,
            CreateNewProcessGroup = 0x00000200,
            CreateNoWindow = 0x08000000,
            CreateProtectedProcess = 0x00040000,
            CreatePreserveCodeAuthzLevel = 0x02000000,
            CreateSeparateWowVdm = 0x00001000,
            CreateSharedWowVdm = 0x00001000,
            CreateSuspended = 0x00000004,
            CreateUnicodeEnvironment = 0x00000400,
            DebugOnlyThisProcess = 0x00000002,
            DebugProcess = 0x00000001,
            DetachedProcess = 0x00000008,
            ExtendedStartupinfoPresent = 0x00080000,
            InheritParentAffinity = 0x00010000
        }

        public static IntPtr ActiveWindow
        {
            get { return GetForegroundWindow(); }
        }

        public static Int64 PerformanceCount
        {
            get
            {
                Int64 result;
                QueryPerformanceCounter(out result);
                return result;
            }
        }

        public static Int64 PerformanceFrequency
        {
            get
            {
                Int64 result;

                if (!QueryPerformanceFrequency(out result))
                {
                    throw new NotSupportedException("This computer does not support HPET.");
                }

                return result;
            }
        }

        [DllImport(Winmm)]
        private static extern UInt32 timeBeginPeriod(UInt32 period);

        [DllImport(Winmm)]
        private static extern UInt32 timeEndPeriod(UInt32 period);

        [DllImport(Kernel32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean QueryPerformanceCounter(out Int64 performanceCount);

        [DllImport(Kernel32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean QueryPerformanceFrequency(out Int64 frequency);

        [DllImport(Kernel32, CharSet = CharSet.Unicode)]
        private static extern IntPtr LoadLibrary(String fileName);

        [DllImport(Kernel32)]
        private static extern Int32 GetPrivateProfileString(String section, String key, String def, StringBuilder retVal, Int32 size, String filePath);

        [DllImport(Kernel32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean WritePrivateProfileString(String lpAppName, String lpKeyName, String lpString, String lpFileName);


        [DllImport(Urlmon, CharSet = CharSet.Ansi)]
        private static extern Int32 UrlMkSetSessionOption(Int32 dwOption, String pBuffer, Int32 dwBufferLength, Int32 dwReserved);

        [DllImport(User32)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport(User32)]
        private static extern Boolean SetForegroundWindow(IntPtr hWnd);

        [DllImport(User32)]
        public static extern Int32 SendMessage(IntPtr hWnd, Int32 msg, Int32 wParam, Int32 lParam);

        [DllImport(User32)]
        public static extern Boolean ReleaseCapture();

        [DllImport(Kernel32, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Boolean CreateProcess(String lpApplicationName, String lpCommandLine, ref SecurityAttributes lpProcessAttributes, ref SecurityAttributes lpThreadAttributes, Boolean bInheritHandles, UInt32 dwCreationFlags, IntPtr lpEnvironment, String lpCurrentDirectory, [In] ref StartupInfo lpStartupInfo, out ProcessInformation lpProcessInformation);

        [DllImport(Kernel32, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        [DllImport(Kernel32, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Boolean WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, Byte[] lpBuffer, Int32 nSize, IntPtr lpNumberOfBytesWritten);

        [DllImport(Kernel32, SetLastError = true)]
        private static extern Boolean ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] Byte[] lpBuffer, Int32 nSize, out Int32 lpNumberOfBytesRead);

        [DllImport(Kernel32, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, UInt32 dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

        [DllImport(Kernel32, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(String lpModuleName);

        [DllImport(Kernel32, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Int32 WaitForSingleObject(IntPtr handle, Int32 wait);

        [DllImport(Kernel32, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, String procName);

        [DllImport(Kernel32, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr LoadLibraryA(String lpFileName);

        [DllImport(Kernel32)]
        private static extern Boolean GetExitCodeThread(IntPtr hThread, out UInt32 lpExitCode);

        private static void ChangeUserAgent(String userAgent)
        {
            UrlMkSetSessionOption(UrlmonOptionUserAgent, userAgent, userAgent.Length, 0);
        }

        public static IntPtr GetProcAddressEx(IntPtr hModule, String procName)
        {
            return GetProcAddress(hModule, procName);
        }

        public static IntPtr GetModuleHandleEx(String moduleName)
        {
            return GetModuleHandle(moduleName);
        }

        public static Int32 WaitForSingleObjectEx(IntPtr handle, Int32 wait)
        {
            return WaitForSingleObject(handle, wait);
        }

        public static IntPtr GetThreadExitCode(IntPtr hThread)
        {
            UInt32 exitCode;

            GetExitCodeThread(hThread, out exitCode);

            return (IntPtr)exitCode;
        }

        public static IntPtr CreateRemoteThreadEx(IntPtr hProcess, IntPtr lpStartAddress, IntPtr lpParameter)
        {
            try
            {
                IntPtr threadAddress = CreateRemoteThread(hProcess, IntPtr.Zero, 0, lpStartAddress, lpParameter, 0, IntPtr.Zero);
                return threadAddress == IntPtr.Zero ? IntPtr.Zero : threadAddress;
            }
            catch (Exception)
            {
                return IntPtr.Zero;
            }
        }

        public static Process CreateProcessEx(String fileName, String arguments, String workingDirectory, ProcessCreationFlags creationFlags)
        {
            try
            {
                StartupInfo startupInfo = new StartupInfo();
                ProcessInformation processInformation;
                SecurityAttributes pSec = new SecurityAttributes();
                SecurityAttributes tSec = new SecurityAttributes();

                pSec.nLength = Marshal.SizeOf(pSec);
                tSec.nLength = Marshal.SizeOf(tSec);

                CreateProcess(fileName, String.Format("{0} {1}", fileName, arguments), ref pSec, ref tSec, false, (UInt32)creationFlags, IntPtr.Zero, workingDirectory, ref startupInfo, out processInformation);

                return processInformation.dwProcessId == 0 ? null : Process.GetProcessById(processInformation.dwProcessId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IntPtr LoadLibraryW(String fileName)
        {
            return LoadLibrary(fileName);
        }

        public static Boolean BeginTimePeriod(UInt32 milliseconds)
        {
            return timeBeginPeriod(milliseconds) <= 0;
        }

        public static Boolean EndTimePeriod(UInt32 milliseconds)
        {
            return timeEndPeriod(milliseconds) <= 0;
        }

        public static Boolean SetPrivateProfileString(String section, String key, String value, String path)
        {
            return WritePrivateProfileString(section, key, value, path);
        }

        public static Boolean GetPrivateProfileBoolean(String section, String key, String path)
        {
            try
            {
                StringBuilder sBuffer = new StringBuilder(8);
                GetPrivateProfileString(section, key, "", sBuffer, 8, path);
                String keyString = sBuffer.ToString().Split(";"[0])[0];

                if (keyString.ToLower() == "true" || keyString.ToLower() == "false")
                {
                    return Convert.ToBoolean(keyString); 
                }

                return Convert.ToBoolean(Convert.ToInt32(keyString));
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static Byte GetPrivateProfileByte(String section, String key, String path)
        {
            try
            {
                StringBuilder sBuffer = new StringBuilder(8);
                GetPrivateProfileString(section, key, "", sBuffer, 8, path);
                String keyString = sBuffer.ToString().Split(";"[0])[0];
                return keyString.Equals("") ? (Byte)0 : Convert.ToByte(keyString);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static Int16 GetPrivateProfileInt16(String section, String key, String path)
        {
            try
            {
                StringBuilder sBuffer = new StringBuilder(16);
                GetPrivateProfileString(section, key, "", sBuffer, 16, path);
                String keyString = sBuffer.ToString().Split(";"[0])[0];
                return keyString.Equals("") ? (Int16)0 : Convert.ToInt16(keyString);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public static Int32 GetPrivateProfileInt32(String section, String key, String path)
        {
            try
            {
                StringBuilder sBuffer = new StringBuilder(32);
                GetPrivateProfileString(section, key, "", sBuffer, 32, path);
                String keyString = sBuffer.ToString().Split(";"[0])[0];
                return keyString.Equals("") ? -1 : Convert.ToInt32(keyString);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public static Single GetPrivateProfileSingle(String section, String key, String path)
        {
            try
            {
                StringBuilder sBuffer = new StringBuilder(32);
                GetPrivateProfileString(section, key, "", sBuffer, 32, path);
                String keyString = sBuffer.ToString().Split(";"[0])[0];
                return keyString.Equals("") ? 0.0f : Convert.ToSingle(keyString);
            }
            catch (Exception)
            {
                return 0.0f;
            }
        }

        public static String GetPrivateProfileString(String section, String key, String path)
        {
            try
            {
                StringBuilder sBuffer = new StringBuilder(255);
                GetPrivateProfileString(section, key, "", sBuffer, 255, path);
                String keyString = sBuffer.ToString().Split(";"[0])[0];
                return keyString;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static String GetBaseDirectory()
        {
            return String.Format("{0}\\", Directory.GetCurrentDirectory());
        }

        public static String GetApplicationInstallLocation(String applicationName)
        {
            String displayName;

            String registryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKey);

            if (key != null)
            {
                RegistryKey key1 = key;

                foreach (RegistryKey subkey in key.GetSubKeyNames().Select(key1.OpenSubKey))
                {
                    displayName = subkey.GetValue("DisplayName") as String;

                    if (displayName != null && displayName.Contains(applicationName))
                    {
                        return subkey.GetValue("InstallLocation") as String;
                    }
                }
                key.Close();
            }

            registryKey = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
            key = Registry.LocalMachine.OpenSubKey(registryKey);

            if (key != null)
            {
                foreach (RegistryKey subkey in key.GetSubKeyNames().Select(keyName => key.OpenSubKey(keyName)))
                {
                    displayName = subkey.GetValue("DisplayName") as String;

                    if (displayName != null && displayName.Contains(applicationName))
                    {
                        return subkey.GetValue("InstallLocation") as String;
                    }
                }
                key.Close();
            }

            return "";
        }

        public static Boolean WriteProcessMemoryEx(IntPtr hProcess, IntPtr baseAddress, Byte[] buffer)
        {
            try
            {
                return WriteProcessMemory(hProcess, baseAddress, buffer, buffer.Length, (IntPtr)0);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static Byte[] ReadProcessMemoryEx(IntPtr hProcess, IntPtr baseAddress, Int32 size)
        {
            try
            {
                Byte[] buffer = new Byte[size];
                Int32 nRead;
                ReadProcessMemory(hProcess, baseAddress, buffer, size, out nRead);
                return buffer;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static IntPtr AllocateMemoryEx(IntPtr hProcess, UInt32 size)
        {
            try
            {
                IntPtr mAddress = VirtualAllocEx(hProcess, IntPtr.Zero, size, AllocationType.Reserve | AllocationType.Commit, MemoryProtection.ExecuteReadWrite);
                return mAddress == IntPtr.Zero ? IntPtr.Zero : mAddress;
            }
            catch (Exception)
            {
                return IntPtr.Zero;
            }
        }
    }
}
