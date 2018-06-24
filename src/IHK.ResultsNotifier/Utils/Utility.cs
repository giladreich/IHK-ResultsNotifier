using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace IHK.ResultsNotifier.Utils
{
    public static class Utility
    {
        public static string TimeStamp => DateTime.Now.ToString("T");


        #region --- Safe Invoke ---

        public static void InvokeSafe(this ISynchronizeInvoke caller, Action method)
        {
            if (caller.InvokeRequired)
                caller.Invoke(method, null);
            else
                method.Invoke();
        }

        public static T InvokeSafe<T>(this ISynchronizeInvoke caller, Func<T> method)
        {
            if (caller.InvokeRequired)
                return (T) caller.Invoke(method, null);
            
            return method.Invoke();
        }

        public static void BeginInvokeSafe(this ISynchronizeInvoke caller, Action method)
        {
            if (caller.InvokeRequired)
                caller.BeginInvoke(method, null);
            else
                method.Invoke();
        }

        #endregion --- Safe Invoke ---


        #region --- Controls Properties/Methods ---

        public static void Text(this Control control, string text)
        {
            control.InvokeSafe(() => control.Text = text);
        }

        public static void Visible(this Control control, bool visible)
        {
            control.InvokeSafe(() => control.Visible = visible);
        }

        public static void Enabled(this Control control, bool enabled)
        {
            control.InvokeSafe(() => control.Enabled = enabled);
        }

        public static void Size(this Control control, Size size)
        {
            control.InvokeSafe(() => control.Size = size);
        }

        public static void Left(this Control control, int position)
        {
            control.InvokeSafe(() => control.Left = position);
        }

        public static void Top(this Control control, int position)
        {
            control.InvokeSafe(() => control.Top = position);
        }

        public static void SendToBack(this Control control, bool safe)
        {
            control.InvokeSafe(control.SendToBack);
        }

        public static void BringToFront(this Control control, bool safe)
        {
            control.InvokeSafe(control.BringToFront);
        }

        public static void Close(this Form form, bool safe)
        {
            form.InvokeSafe(form.Close);
        }

        #endregion  --- Controls Properties/Methods ---


        #region --- Tasks ---

        public static async Task SimulateWork(int miliseconds)
        {
            await Task.Run(() => Thread.Sleep(miliseconds));
        }

        public static async Task SimulateWork(TimeSpan timeSpan)
        {
            await Task.Run(() => Thread.Sleep(timeSpan));
        }

        #endregion --- Tasks ---
    }
}
