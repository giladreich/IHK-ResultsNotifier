using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace IHK.ResultsNotifier.Utils
{
    public static class Utility
    {
        public static string TimeStamp => DateTime.Now.ToString("T");

        public static void InvokeSafe<T>(this T control, Action action)
            where T : Control, ISynchronizeInvoke
        {
            if (control.InvokeRequired)
                control.Invoke(action);
            else
                action.Invoke();
        }

        public static void BeginInvokeSafe<T>(this T control, Action action)
            where T : Control, ISynchronizeInvoke
        {
            if (control.InvokeRequired)
                control.BeginInvoke(action);
            else
                action.Invoke();
        }

        public static Task<T> StartTask<T>(Func<T> function)
        {
            return Task.Factory.StartNew(function);
        }

        public static async Task<int> SimulateWork(int miliseconds)
        {
            await Task.Factory.StartNew(() => Thread.Sleep(miliseconds));
            return 1;
        }

    }
}
