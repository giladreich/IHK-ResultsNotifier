using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using IHK.ResultsNotifier.Windows;

[assembly:Fody.ConfigureAwait(false)]

namespace IHK.ResultsNotifier
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginWindow());
        }
    }
}
