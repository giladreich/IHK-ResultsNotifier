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

        public static void Text(this Control control, string text)
        {
            if (control.InvokeRequired)
                control.InvokeSafe(() => control.Text = text);
            else
                control.Text = text;
        }

        public static void Visible(this Control control, bool visible)
        {
            Action action = () =>
            {
                if (visible) control.Show();
                else control.Hide();
            };


            if (control.InvokeRequired)
                control.InvokeSafe(action);
            else
                action.Invoke();
        }

        public static Task<T> StartTask<T>(Func<T> function)
        {
            return Task.Factory.StartNew(function);
        }

        public static async Task SimulateWork(int miliseconds)
        {
            await Task.Factory.StartNew(() => Thread.Sleep(miliseconds));
        }

    }
}
