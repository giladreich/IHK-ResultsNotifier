using System;
using System.Runtime.InteropServices;
using System.Security;


namespace IHK.ResultsNotifier.Utils
{
    [ComVisible(false), SuppressUnmanagedCodeSecurity]
    internal class NativeMethods
    {
        [DllImport("user32.dll")]
        internal static extern int SetForegroundWindow(IntPtr hWnd);
    }
}
