using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace FinanceSharp.Data {
    /// <summary>
    ///     A thread 
    /// </summary>
    public class DisposerThread : IDisposable {
        protected BusyBlockingCollection<object> queue = new BusyBlockingCollection<object>();
        protected Thread thread;
        protected CancellationTokenSource source;
        public static DisposerThread Instance;
        protected static BusyBlockingCollection<object> InstanceQueue;

        static DisposerThread() {
            Instance = new DisposerThread();
            InstanceQueue = Instance.queue;
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        protected DisposerThread() {
            source = new CancellationTokenSource();
            thread = new Thread(Consumer);
            thread.Name = "DisposerThread";
            thread.Start();
        }

        private void Consumer() {
            var token = source.Token;
            foreach (var o in queue.GetConsumingEnumerable(token)) {
                switch (o) {
                    case IntPtr ptr:
                        Marshal.FreeHGlobal(ptr);
                        break;
                    case GCHandle handle:
                        handle.Free();
                        break;
                    case null:
                        break;
                    case Action act:
                        act();
                        break;
                }
            }
        }

        public void Cancel() {
            source.Cancel();
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose() {
            queue?.Dispose();
            source.Cancel();
        }

        public static void Enqueue(IntPtr ptr) {
            InstanceQueue.Add(ptr);
        }

        public static void Enqueue(GCHandle ptr) {
            InstanceQueue.Add(ptr);
        }

        public static void Enqueue(Action work) {
            InstanceQueue.Add(work);
        }

        public static unsafe void Enqueue(void* ptr) {
            InstanceQueue.Add(new IntPtr(ptr));
        }
    }
}