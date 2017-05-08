using System;
using System.Runtime.InteropServices;

namespace Tasks.InteroperatingWithUnmanagedCode.COM
{
    [ComVisible(true)]
    [Guid("E54F8365-D5EB-4C2B-A3FE-7C2B7EDF902A")]
    [ClassInterface(ClassInterfaceType.None)]
    public sealed class PowerStateManagerCOM : IPowerStateManagerCOM
    {
        private readonly PowerStateManager m_PowerManager = new PowerStateManager();

        public DateTime GetLastSleepTime()
        {
            return m_PowerManager.GetLastSleepTime();
        }
        public DateTime GetLastWakeTime()
        {
            return m_PowerManager.GetLastWakeTime();
        }
    }
}
