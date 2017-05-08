using System;
using System.Runtime.InteropServices;

namespace Tasks.InteroperatingWithUnmanagedCode
{
    [StructLayout(LayoutKind.Sequential)]
    [ComVisible(true)]
    public struct SystemBatteryState
    {
        public bool AcOnLine;
        public bool BatteryPresent;
        public bool Charging;
        public bool Discharging;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public bool[] Spare1;
        public int MaxCapacity;
        public int RemainingCapacity;
        public int Rate;
        public int EstimatedTime;
        public int DefaultAlert1;
        public int DefaultAlert2;

        public override string ToString() => $"AcOnLine: {AcOnLine}, BatteryPresent: {BatteryPresent}, " +
                                             $"Charging: {Charging}, Discharging: {Discharging}, " +
                                             $"Spare1 {string.Join("|", Spare1)}, " +
                                             $"MaxCapacity: {MaxCapacity}, RemainingCapacity: {RemainingCapacity}, " +
                                             $"Rate: {Rate}, EstimatedTime: {EstimatedTime}, " +
                                             $"DefaultAlert1: {DefaultAlert1}, DefaultAlert2: {DefaultAlert2}";
    }

    [StructLayout(LayoutKind.Sequential)]
    [ComVisible(true)]
    public struct SystemPowerInformation
    {
        public uint MaxIdlenessAllowed;
        public uint Idleness;
        public uint TimeRemaining;
        public sbyte CoolingMode;

        public override string ToString() => $"MaxIdlenessAllowed: {MaxIdlenessAllowed}, Idleness: {Idleness}, " +
                                             $"TimeRemaining: {TimeSpan.FromTicks(TimeRemaining)}, CoolingMode: {CoolingMode}";
    }

    public enum InformationLevel
    {
        SystemBatteryState = 5,
        SystemReserveHiberFile = 10,
        SystemPowerInformation = 12,
        LastWakeTime = 14,
        LastSleepTime = 15
    }

    public sealed class PowerStateManager
    {
        public DateTime GetLastSleepTime()
        {
            return GetLastTime(InformationLevel.LastSleepTime);
        }

        public DateTime GetLastWakeTime()
        {
            return GetLastTime(InformationLevel.LastWakeTime);
        }

        public SystemBatteryState GetSystemBatteryState()
        {
            return GetSystemInformation<SystemBatteryState>(InformationLevel.SystemBatteryState);
        }

        public SystemPowerInformation GetSystemPowerInformation()
        {
            return GetSystemInformation<SystemPowerInformation>(InformationLevel.SystemPowerInformation);
        }

        public void ReserveHibernationFile()
        {
            ManageHibernationFile(true);
        }

        public void RemoveHibernationFile()
        {
            ManageHibernationFile(false);
        }

        public void SleepSystem()
        {
            SuspendSystem(false);
        }

        public void HibernateSystem()
        {
            SuspendSystem(true);
        }

        private void SuspendSystem(bool hibernate)
        {
            PowerStateManagement.SetSuspendState(hibernate, false, false);
        }

        private void ManageHibernationFile(bool reserve)
        {
            var inputBufferSize = Marshal.SizeOf<int>();
            var inputBuffer = Marshal.AllocHGlobal(inputBufferSize);

            Marshal.WriteInt32(inputBuffer, 0, reserve ? 1 : 0);
            PowerStateManagement.CallNtPowerInformation((int)InformationLevel.SystemReserveHiberFile, inputBuffer, (uint)inputBufferSize, IntPtr.Zero, 0);

            Marshal.FreeHGlobal(inputBuffer);
        }

        private static DateTime GetLastTime(InformationLevel level)
        {
            long ticks = 0;
            CallNtPowerInformation<ulong>(level, buffer => ticks = Marshal.ReadInt64(buffer));

            var startupTime = PowerStateManagement.GetTickCount64() * 10000;
            var date = DateTime.UtcNow - TimeSpan.FromTicks((long)startupTime) + TimeSpan.FromTicks(ticks);

            return date;
        }

        private static T GetSystemInformation<T>(InformationLevel level)
        {
            var information = default(T);
            CallNtPowerInformation<T>(level, buffer => information = Marshal.PtrToStructure<T>(buffer));

            return information;
        }

        private static void CallNtPowerInformation<T>(InformationLevel level, Action<IntPtr> readOutputBuffer)
        {
            var outputBufferSize = Marshal.SizeOf<T>();
            var outputBuffer = Marshal.AllocHGlobal(outputBufferSize);

            PowerStateManagement.CallNtPowerInformation((int)level, IntPtr.Zero, 0, outputBuffer, (uint)outputBufferSize);
            readOutputBuffer.Invoke(outputBuffer);
            Marshal.FreeHGlobal(outputBuffer);
        }

        #region Dll Imports
        private class PowerStateManagement
        {
            [DllImport("kernel32")]
            public static extern ulong GetTickCount64();
            
            [DllImport("powrprof.dll")]
            public static extern uint CallNtPowerInformation(
                int informationLevel,
                IntPtr inputBuffer,
                uint inputBufferSize,
                [Out] IntPtr outputBuffer,
                uint outputBufferSize);

            [DllImport("powrprof.dll")]
            public static extern bool SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);
        }
        #endregion
    }
}
