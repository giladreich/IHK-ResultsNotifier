using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using IHK.ResultsNotifier.Utils;

namespace IHK.ResultsNotifier.Misc
{
    public class Loader
    {
        public static Form ParentWindow { get; set; }

        private static LoaderWindow loaderWindow;


        public static void StartLoader(int sleepMiliseconds = 500)
        {
            if (ParentWindow == null)
                throw new InvalidOperationException(
                    "You must first set the ParentWindow property before invoking a loader.");

            InvokeLoader(loaderWindow ?? (loaderWindow = new LoaderWindow()));

            Thread.Sleep(sleepMiliseconds);
        }

        public static void StopLoader(IntPtr windowHandle)
        {
            if (loaderWindow == null) return;

            loaderWindow.InvokeSafe(() => loaderWindow.Close());
            ActivateWindow(windowHandle);
        }




        private static void InvokeLoader(Form window)
        {
            if (window.Visible) return;

            ForceFocusHandling(window);
            CenterWindowToParent(window);

            new Worker(() => window.ShowDialog()).Start();
        }

        private static void ForceFocusHandling(Form window)
        {
            window.LostFocus -= null;
            window.LostFocus += (sender, e) => window.Focus();
        }

        private static void CenterWindowToParent(Form window)
        {
            window.StartPosition = FormStartPosition.Manual;

            int posX = ParentWindow.Location.X + ParentWindow.Width  / 2 - window.Width  / 2;
            int posY = ParentWindow.Location.Y + ParentWindow.Height / 2 - window.Height / 2;
            window.Location = new Point(posX - 20, posY);
        }

        private static void ActivateWindow(IntPtr handle)
        {
            NativeMethods.SetForegroundWindow(handle);
        }

    }
}
