using System;

namespace FinanceSharp.Data {
    public unsafe partial class DoubleArray {
        private void ReleaseUnmanagedResources() {
            DisposerThread.Enqueue(Address);
        }

        protected virtual void Dispose(bool disposing) {
            ReleaseUnmanagedResources();
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.</summary>
        ~DoubleArray() {
            Dispose(false);
        }
    }
}