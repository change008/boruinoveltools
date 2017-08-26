using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Tiexue.Framework.Utils {
    /// <summary>
    /// 维护与本地计算机系统有关的设置,封装了对Windows动态库的调用。
    /// </summary>
    public static class LocalSystemUtil {
        [DllImport("kernel32")]
        private static extern void GetWindowsDirectory(StringBuilder winDir, int count);

        [DllImport("kernel32")]
        private static extern void GetSystemDirectory(StringBuilder sysDir, int count);

        [DllImport("kernel32")]
        private static extern void GetSystemInfo(ref CpuInfo cpuinfo);

        [DllImport("kernel32")]
        private static extern void GlobalMemoryStatus(ref MemoryInfo meminfo);

        [DllImport("kernel32")]
        private static extern void GetSystemTime(ref SystemTimeInfo stinfo);

        [DllImport("kernel32")]
        private static extern void SetSystemTime(ref SystemTimeInfo stinfo);

        /// <summary>
        /// Windows目录
        /// </summary>
        public static string WindowsDirectory {
            get {
                const int nChars = 128;
                var buff = new StringBuilder(nChars);
                GetWindowsDirectory(buff, nChars);
                return buff.ToString();
            }
        }

        /// <summary>
        /// 系统目录
        /// </summary>
        public static string SystemDirectory {
            get {
                const int nChars = 128;
                var buff = new StringBuilder(nChars);
                GetSystemDirectory(buff, nChars);
                return buff.ToString();
            }
        }

        /// <summary>
        /// CPU信息
        /// </summary>
        public static CpuInfo SystemInfo {
            get {
                CpuInfo cpuInfo = new CpuInfo();
                GetSystemInfo(ref cpuInfo);
                return cpuInfo;
            }
        }

        /// <summary>
        /// 内存信息
        /// </summary>
        public static MemoryInfo MemoryStatus {
            get {
                var memInfo = new MemoryInfo();
                GlobalMemoryStatus(ref memInfo);
                return memInfo;
            }
        }

        /// <summary>
        /// 日期信息
        /// </summary>
        public static DateTime SystemTime {
            get {
                var stInfo = new SystemTimeInfo();
                GetSystemTime(ref stInfo);
                var dt = new DateTime(stInfo.wYear, stInfo.wMonth, stInfo.wDay, stInfo.wHour, stInfo.wMinute, stInfo.wSecond, stInfo.wMilliseconds).ToLocalTime();
                return dt;
            }
            set {
                var stInfo = new SystemTimeInfo();
                var vvvv = value.ToUniversalTime();
                stInfo.wYear = (ushort) vvvv.Year;
                stInfo.wMonth = (ushort) vvvv.Month;
                stInfo.wDay = (ushort) vvvv.Day;
                stInfo.wHour = (ushort) vvvv.Hour;
                stInfo.wMinute = (ushort) vvvv.Minute;
                stInfo.wSecond = (ushort) vvvv.Second;
                stInfo.wMilliseconds = (ushort) vvvv.Millisecond;
                SetSystemTime(ref stInfo);
            }
        }
    }

    /// <summary>
    /// 定义CPU的信息结构
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CpuInfo {
        /// <summary>
        /// 
        /// </summary>
        public uint dwOemId;

        /// <summary>
        /// 
        /// </summary>
        public uint dwPageSize;

        /// <summary>
        /// 
        /// </summary>
        public uint lpMinimumApplicationAddress;

        /// <summary>
        /// 
        /// </summary>
        public uint lpMaximumApplicationAddress;

        /// <summary>
        /// 
        /// </summary>
        public uint dwActiveProcessorMask;

        /// <summary>
        /// 
        /// </summary>
        public uint dwNumberOfProcessors;

        /// <summary>
        /// 
        /// </summary>
        public uint dwProcessorType;

        /// <summary>
        /// 
        /// </summary>
        public uint dwAllocationGranularity;

        /// <summary>
        /// 
        /// </summary>
        public uint dwProcessorLevel;

        /// <summary>
        /// 
        /// </summary>
        public uint dwProcessorRevision;
    }

    /// <summary>
    /// 定义内存的信息结构
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MemoryInfo {
        /// <summary>
        /// 
        /// </summary>
        public uint dwLength;

        /// <summary>
        /// 
        /// </summary>
        public uint dwMemoryLoad;

        /// <summary>
        /// 
        /// </summary>
        public uint dwTotalPhys;

        /// <summary>
        /// 
        /// </summary>
        public uint dwAvailPhys;

        /// <summary>
        /// 
        /// </summary>
        public uint dwTotalPageFile;

        /// <summary>
        /// 
        /// </summary>
        public uint dwAvailPageFile;

        /// <summary>
        /// 
        /// </summary>
        public uint dwTotalVirtual;

        /// <summary>
        /// 
        /// </summary>
        public uint dwAvailVirtual;
    }

    /// <summary>
    /// 定义系统时间的信息结构
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SystemTimeInfo {
        /// <summary>
        /// 
        /// </summary>
        public ushort wYear;

        /// <summary>
        /// 
        /// </summary>
        public ushort wMonth;

        /// <summary>
        /// 
        /// </summary>
        public ushort wDayOfWeek;

        /// <summary>
        /// 
        /// </summary>
        public ushort wDay;

        /// <summary>
        /// 
        /// </summary>
        public ushort wHour;

        /// <summary>
        /// 
        /// </summary>
        public ushort wMinute;

        /// <summary>
        /// 
        /// </summary>
        public ushort wSecond;

        /// <summary>
        /// 
        /// </summary>
        public ushort wMilliseconds;
    }
}
