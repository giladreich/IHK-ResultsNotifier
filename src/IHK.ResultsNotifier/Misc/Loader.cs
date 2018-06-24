using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

using IHK.ResultsNotifier.Utils;


namespace IHK.ResultsNotifier.Misc
{
    public class Loader
    {
        public static Form Owner { get; set; }

        private static Form loaderWindow;

        private static readonly Mutex mutex = new Mutex(true, "LoaderMutex");
        private static readonly object locker = new object();


        /// <param name="sleepMiliseconds">The time to sleep the main UI thread.</param>
        public static void Start(int sleepMiliseconds = 500)
        {
            if (Owner == null)
                throw new InvalidOperationException(
                    "You must first set the Owner property before invoking a loader.");

            mutex.WaitOne();

            lock (locker)
            {
                InvokeLoader(loaderWindow ?? (loaderWindow = new LoaderWindow()));

                Owner.LocationChanged -= null;
                Owner.LocationChanged += (sender, args) => loaderWindow.InvokeSafe(() => CenterWindowToOwner(loaderWindow));

                Thread.Sleep(sleepMiliseconds);
            }
        }

        /// <param name="hActivate">Pass a window handle if you want to bring the window to front. </param>
        public static void Stop(IntPtr hActivate = default(IntPtr))
        {
            if (loaderWindow == null) return;

            loaderWindow.InvokeSafe(() => loaderWindow.Close());

            if (hActivate != default(IntPtr))
                ActivateWindow(hActivate);

            mutex.ReleaseMutex();
        }




        private static void InvokeLoader(Form window)
        {
            if (window.Visible) return;

            ForceFocusHandling(window);
            CenterWindowToOwner(window);

            Worker worker = new Worker(() => window.ShowDialog());
            Console.WriteLine("Invoking Loader on ThreadId -> " + worker.ThreadId);
            worker.Start();
        }

        private static void ForceFocusHandling(Form window)
        {
            window.LostFocus -= null;
            window.LostFocus += (sender, e) => window.Focus();
        }

        private static void CenterWindowToOwner(Form window)
        {
            window.StartPosition = FormStartPosition.Manual;

            int posX = Owner.Location.X + Owner.Width  / 2 - window.Width  / 2;
            int posY = Owner.Location.Y + Owner.Height / 2 - window.Height / 2;
            window.Location = new Point(posX - 10, posY);
        }

        private static void ActivateWindow(IntPtr handle)
        {
            NativeMethods.SetForegroundWindow(handle);
        }

    }
}
