using System;
using System.Threading;
using System.Windows.Forms;

namespace IHK.ResultsNotifier.Utils
{
    public enum State { Fresh, Done, Busy, Sleeping }

    public class Worker : IDisposable
    {
        private Thread worker;
        private AutoResetEvent token;


        public State State { get; set; } = State.Fresh;
        public AutoResetEvent ThreadToken => token;
        public bool IsWorking { get; set; }
        public bool IsBackground
        {
            get => worker.IsBackground;
            set => worker.IsBackground = value;
        }

        public int ThreadId => worker.ManagedThreadId;


        public Worker(MethodInvoker callback)
        {
            worker = new Thread(new ThreadStart(callback));
        }




        public Worker Start()
        {
            State = State.Busy;
            IsWorking = true;
            token = new AutoResetEvent(false);

            worker.Start();

            return this;
        }

        public void Stop()
        {
            token.Set();
            IsWorking = false;
            State = State.Done;
            Join();
        }


        public void Join()
        {
            worker.Join();
        }

        public void Dispose()
        {
            token?.Dispose();
            worker = null;
        }

    }
}
