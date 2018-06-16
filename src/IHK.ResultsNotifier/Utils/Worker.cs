using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IHK.ResultsNotifier.Utils
{
    public enum State { Fresh, Done, Busy, Sleeping }


    public class Worker : IDisposable
    {
        private Thread worker;

        public State State { get; set; } = State.Fresh;

        public Worker(MethodInvoker callback)
        {
            worker = new Thread(new ThreadStart(callback));
        }



        public Worker Start()
        {
            State = State.Busy;

            worker.Start();

            return this;
        }

        public void Stop()
        {

        }


        public void Dispose()
        {
            worker = null;
        }
    }
}
