using System;
using System.Threading;


namespace IHK.ResultsNotifier.Utils
{
    public enum State { Fresh, Done, Busy, Sleeping, Paused, Waiting }

    public class Worker : IDisposable
    {
        public State State { get; protected set; } = State.Fresh;
        public bool IsWorking { get; protected set; }
        public int ThreadId => Thread.ManagedThreadId;
        public string Name
        {
            get => Thread.Name;
            set => Thread.Name = value;
        }
        public bool IsBackground
        {
            get => Thread.IsBackground;
            set => Thread.IsBackground = value;
        }
        public ThreadPriority ThreadPriority
        {
            get => Thread.Priority;
            set => Thread.Priority = value;
        }
        public ApartmentState ApartmentState
        {
            get => Thread.GetApartmentState();
            set => Thread.SetApartmentState(value);
        }



        protected Thread Thread;
        protected AutoResetEvent Token;



        public Worker(Action callback)
        {
            Thread = new Thread(new ThreadStart(callback));
        }


        public virtual Worker Start()
        {
            Token = new AutoResetEvent(false);
            Thread.Start();

            State = State.Busy;
            IsWorking = true;

            return this;
        }

        public virtual Worker Start(bool isJoinedThread)
        {
            Start();

            if (isJoinedThread)
                Join();

            return this;
        }

        public virtual void Stop()
        {
            if (Token == null || !IsWorking)
                throw new InvalidOperationException("Thread didn't start, no reason to call stop.");

            IsWorking = false;
            State = State.Done;

            Token.Set();
            Join();
        }

        public virtual void Continue()
        {
            if (Token == null || State != State.Paused)
                throw new InvalidOperationException("Thread didn't pause, no reason to call continue.");

            Token.Set();
            IsWorking = true;
            State = State.Busy;
        }

        public virtual void Pause()
        {
            if (State == State.Paused) return;

            Sleep();
            IsWorking = false;
            State = State.Paused;
        }

        public virtual void Join()
        {
            State = State.Waiting;
            Thread.Join();
        }

        public bool Sleep() => 
            SleepDelegate(() => Token.WaitOne());

        public bool Sleep(TimeSpan timeout) => 
            SleepDelegate(() => Token.WaitOne(timeout));

        public bool Sleep(TimeSpan timeout, bool exitContext) => 
            SleepDelegate(() => Token.WaitOne(timeout, exitContext));

        public bool Sleep(int millisecondsTimeout) => 
            SleepDelegate(() => Token.WaitOne(millisecondsTimeout));

        public bool Sleep(int millisecondsTimeout, bool exitContext) => 
            SleepDelegate(() => Token.WaitOne(millisecondsTimeout, exitContext));

        private bool SleepDelegate(Func<bool> callback)
        {
            State = State.Sleeping;
            bool result = callback.Invoke();
            State = State.Busy;

            return result;
        }

        public virtual void Dispose()
        {
            Token?.Dispose();
            Thread = null;
        }

    }
}
