using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Helper
{
    public class ListCollection<T> : List<T>
    {
        private readonly Object _syncRoot = new Object();

        public Object SyncRoot
        {
            get { return _syncRoot; }
        }

        public new T this[Int32 index]
        {
            get { return (index < 0) || ((Count - index) <= 0) ? default(T) : base[index]; }
        }

        public ListCollection()
        {
        }

        public ListCollection(IEnumerable<T> item)
        {
            AddRange(item);
        }
    }

    public static class ExtException
    {
        public static String GetStackTrace(this Exception exception)
        {
            return String.Format("Exception:\n{0}\nStack Trace:\n{1}", exception, exception.StackTrace);
        }
    }

    public static class ExtDateTime
    {
        public static Int32 GetUnixTime(this DateTime dateTime)
        {
			return (Int32)(dateTime - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }

        public static DateTime StartOfWeek(this DateTime dateTime)
        {
            int diff = dateTime.DayOfWeek - DayOfWeek.Monday;

            if (diff < 0)
            {
                diff += 7;
            }

            return dateTime.AddDays(-1 * diff).Date;
        }
    }

    public static class ExtString
    {
        /// <summary>Encodes the string into Base64 and returns the result.</summary>
        public static String EncodeBase64(this String value)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
        }

        /// <summary>Decodes the string from Base64 and returns the result.</summary>
        public static String DecodeBase64(this String value)
        {
            return Encoding.Default.GetString(Convert.FromBase64String(value));
        }

        /// <summary>Encrypts the string and returns the result.</summary>
        public static Byte[] Encrypt(this String value)
        {
            Byte[] buffer = new Byte[value.Length];

            for (Int32 i = 0; i < value.Length; i++)
            {
                buffer[i] = (Byte) (value[i] ^ (0x54 ^ (i & 0xFF)));
            }

            return buffer;
        }

        /// <summary>Decrypts the string and returns the result.</summary>
        public static String Decrypt(this Byte[] value)
        {
            StringBuilder sBuilder = new StringBuilder();

            for (Int32 i = 0; i < value.Length; i++)
            {
                sBuilder.Append((Char) (value[i] ^ (0x54 ^ (i & 0xFF))));
            }

            return sBuilder.ToString();
        }

        public static String Escape(this String value)
        {
            return value == null ? null : Regex.Replace(value, @"[\r\n\x00\x1a\\'""]", @"\$0");
        }

		public static Boolean IsNumber(this String value, Boolean allowDecimals)
		{
			if (allowDecimals)
			{
				double num;
				return double.TryParse(value, out num);
			}
			else
			{
				long num;
				return long.TryParse(value, out num);
			}
		}

	    public static String GetFirstAOrAnPrefix(this String value)
        {
            if (value == "11") return "an";

            Char ch = value[0];

            switch (ch)
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '9':
                {
                    return "a";
                }
                case '8':
                {
                    return "an";
                }
                default:
                {
                    return "a";
                }
            }
        }
    }

    public static class ExtProcess
    {
        public static IntPtr CreateThread(this Process p, IntPtr lpStartAddress, IntPtr lpParameter)
        {
            return NativeMethods.CreateRemoteThreadEx(p.Handle, lpStartAddress, lpParameter);
        }

        public static Process StartSuspended(this Process p)
        {
            return NativeMethods.CreateProcessEx(p.StartInfo.FileName, p.StartInfo.Arguments, p.StartInfo.WorkingDirectory, NativeMethods.ProcessCreationFlags.CreateSuspended);
        }
        public static Process StartEx(this Process p)
        {
            return NativeMethods.CreateProcessEx(p.StartInfo.FileName, p.StartInfo.Arguments, p.StartInfo.WorkingDirectory, NativeMethods.ProcessCreationFlags.CreateDefaultErrorMode);
        }

        public static Boolean WriteMemory(this Process p, IntPtr baseAddress, Byte[] buffer)
        {
            try
            {
                return NativeMethods.WriteProcessMemoryEx(p.Handle, baseAddress, buffer);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static Byte[] ReadMemory(this Process p, IntPtr baseAddress, Int32 size)
        {
            try
            {
                Byte[] buffer = new Byte[size];
                NativeMethods.ReadProcessMemoryEx(p.Handle, baseAddress, size);
                return buffer;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static IntPtr AllocateMemory(this Process p, UInt32 size)
        {
            try
            {
                IntPtr mAddress = NativeMethods.AllocateMemoryEx(p.Handle, size);
                return mAddress == IntPtr.Zero ? IntPtr.Zero : mAddress;
            }
            catch (Exception)
            {
                return IntPtr.Zero;
            }
        }

        public static Boolean InjectHook(this Process p)
        {
            String hookFileName = String.Format("{0}\\{1}", NativeMethods.GetBaseDirectory(), "Hook.dll");

            if (!File.Exists(hookFileName))
            {
                MessageBox.Show("Hook.dll is missing. Please place Hook.dll in the directory.", "Loader", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            Byte[] hookPath = Encoding.ASCII.GetBytes(hookFileName);

            IntPtr pHookPathString = AllocateMemory(p, Convert.ToUInt32(hookPath.Length));

            WriteMemory(p, pHookPathString, hookPath);

            IntPtr pKernel32 = NativeMethods.GetModuleHandleEx(NativeMethods.Kernel32);
            IntPtr pLoadLibraryA = NativeMethods.GetProcAddressEx(pKernel32, "LoadLibraryA");

            IntPtr hThread = CreateThread(p, pLoadLibraryA, pHookPathString);
            Int32 threadResult = NativeMethods.WaitForSingleObjectEx(hThread, 5000);
            IntPtr hookBase = NativeMethods.GetThreadExitCode(hThread);

            IntPtr pInstallHook = GetHookFuncAddress(p, hookBase, "?InstallHook@@YGXXZ");

            MemoryStream iStream = new MemoryStream(128);

            // MOV EAX, pInstallHook 
            iStream.WriteByte(0xB8);
            iStream.Write(BitConverter.GetBytes((Int32)pInstallHook), 0, 4);

            // CALL EAX
            iStream.WriteByte(0xFF);
            iStream.WriteByte(0xD0);

            // RETN
            iStream.WriteByte(0xC3);

            IntPtr pThreadInstallHook = AllocateMemory(p, 128);
            WriteMemory(p, pThreadInstallHook, iStream.ToArray());

            hThread = CreateThread(p, pThreadInstallHook, (IntPtr)0);
            NativeMethods.WaitForSingleObjectEx(hThread, 5000);

            if (threadResult != 0x0)
            {
                p.Kill();
                return false;
            }

            return true;
        }

        private static IntPtr GetHookFuncAddress(Process process, IntPtr baseAddress, String functionName)
        {
            IntPtr pKernel32 = NativeMethods.GetModuleHandleEx(NativeMethods.Kernel32);
            IntPtr pGetProcAddress = NativeMethods.GetProcAddressEx(pKernel32, "GetProcAddress");

            Byte[] funcBytes = Encoding.ASCII.GetBytes(functionName);
            IntPtr pInstallHookString = AllocateMemory(process, Convert.ToUInt32(funcBytes.Length));

            WriteMemory(process, pInstallHookString, funcBytes);

            MemoryStream iStream = new MemoryStream(128);

            // PUSH pInstallHookString
            iStream.WriteByte(0x68);
            iStream.Write(BitConverter.GetBytes((Int32)pInstallHookString), 0, 4);

            // PUSH hookBase
            iStream.WriteByte(0x68);
            iStream.Write(BitConverter.GetBytes((Int32)baseAddress), 0, 4);

            // MOV EAX, GetProcAddress
            iStream.WriteByte(0xB8);
            iStream.Write(BitConverter.GetBytes((Int32)pGetProcAddress), 0, 4);

            // CALL EAX
            iStream.WriteByte(0xFF);
            iStream.WriteByte(0xD0);

            // RETN
            iStream.WriteByte(0xC3);

            IntPtr pGetFuncAddress = AllocateMemory(process, 128);
            WriteMemory(process, pGetFuncAddress, iStream.ToArray());

            IntPtr hThread = CreateThread(process, pGetFuncAddress, (IntPtr)0);
            NativeMethods.WaitForSingleObjectEx(hThread, 5000);

            return NativeMethods.GetThreadExitCode(hThread);
        }
    }
}