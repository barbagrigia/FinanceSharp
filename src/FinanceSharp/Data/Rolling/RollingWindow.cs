/*
 * All Rights reserved to Ebby Technologies LTD @ Eli Belash, 2020.
 * Original code by: 
 * 
 * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using static FinanceSharp.StringExtensions;

namespace FinanceSharp.Data.Rolling {
    /// <summary>
    ///     This is a window that allows for list access semantics,
    ///     where this[0] refers to the most recent item in the
    ///     window and this[Count-1] refers to the last item in the window
    /// </summary>
    /// <typeparam name="T">The type of data in the window</typeparam>
    public class RollingWindow<T> : IReadOnlyWindow<T> {
        // the backing list object used to hold the data
        private readonly List<T> _list;

        // read-write lock used for controlling access to the underlying list data structure
        private readonly ReaderWriterLockSlim _listLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        // the most recently removed item from the window (fell off the back)
        private T _mostRecentlyRemoved;

        // the total number of samples taken by this indicator
        private long _samples;

        // used to locate the last item in the window as an indexer into the _list
        private int _tail;

        /// <summary>
        ///     Initializes a new instance of the RollwingWindow class with the specified window size.
        /// </summary>
        /// <param name="size">The number of items to hold in the window</param>
        public RollingWindow(int size, string name = "") {
            if (size < 1) {
                throw new ArgumentException("RollingWindow must have size of at least 1.", nameof(size));
            }

            _list = new List<T>(size);
            Size = size;
            Name = name;
        }

        /// <summary>
        ///     Gets the size of this window
        /// </summary>
        public int Size { get; }

        /// <summary>
        ///     Gets the current number of elements in this window
        /// </summary>
        public int Count {
            get {
                try {
                    _listLock.EnterReadLock();
                    return _list.Count;
                } finally {
                    _listLock.ExitReadLock();
                }
            }
        }

        /// <summary>
        /// 	 Gets a name for this indicator
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Gets the number of samples that have been added to this window over its lifetime
        /// </summary>
        public long Samples {
            get {
                try {
                    _listLock.EnterReadLock();
                    return _samples;
                } finally {
                    _listLock.ExitReadLock();
                }
            }
        }

        /// <summary>
        ///     Gets the most recently removed item from the window. This is the
        ///     piece of data that just 'fell off' as a result of the most recent
        ///     add. If no items have been removed, this will throw an exception.
        /// </summary>
        public T MostRecentlyRemoved {
            get {
                try {
                    _listLock.EnterReadLock();

                    if (Samples <= Size) {
                        throw new InvalidOperationException("No items have been removed yet!");
                    }

                    return _mostRecentlyRemoved;
                } finally {
                    _listLock.ExitReadLock();
                }
            }
        }

        /// <summary>
        ///     Indexes into this window, where index 0 is the most recently
        ///     entered value
        /// </summary>
        /// <param name="i">the index, i</param>
        /// <returns>the ith most recent entry</returns>
        public T this[int i] {
            get {
                try {
                    _listLock.EnterReadLock();

                    if (Count == 0) {
                        throw new ArgumentOutOfRangeException(nameof(i), "Rolling window is empty");
                    } else if (i > Size - 1 || i < 0) {
                        throw new ArgumentOutOfRangeException(nameof(i), i,
                            Invariant($"Index must be between 0 and {Size - 1} (rolling window is of size {Size})")
                        );
                    } else if (i > Count - 1) {
                        throw new ArgumentOutOfRangeException(nameof(i), i,
                            Invariant($"Index must be between 0 and {Count - 1} (entry {i} does not exist yet)")
                        );
                    }

                    return _list[(Count + _tail - i - 1) % Count];
                } finally {
                    _listLock.ExitReadLock();
                }
            }
            set {
                try {
                    _listLock.EnterWriteLock();

                    if (i < 0 || i > Count - 1) {
                        throw new ArgumentOutOfRangeException(nameof(i), i, Invariant($"Must be between 0 and {Count - 1}"));
                    }

                    _list[(Count + _tail - i - 1) % Count] = value;
                } finally {
                    _listLock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        ///     Gets a value indicating whether or not this window is ready, i.e,
        ///     it has been filled to its capacity
        /// </summary>
        public bool IsReady {
            get {
                try {
                    _listLock.EnterReadLock();
                    return Samples >= Size;
                } finally {
                    _listLock.ExitReadLock();
                }
            }
        }

        /// <summary>
        /// 	 Gets the current state of this updatable. If the state has not been updated
        /// 	 then the value will be null.
        /// </summary>
        public DoubleArray Current { get; protected set; }

        /// <summary>
        /// 	 Gets the current time of <see cref="IUpdatable.Current"/>.
        /// </summary>
        public long CurrentTime { get; protected set; }

        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<T> GetEnumerator() {
            // we make a copy on purpose so the enumerator isn't tied
            // to a mutable object, well it is still mutable but out of scope
            var temp = new List<T>(Count);
            try {
                _listLock.EnterReadLock();

                for (int i = 0; i < Count; i++) {
                    temp.Add(this[i]);
                }

                return temp.GetEnumerator();
            } finally {
                _listLock.ExitReadLock();
            }
        }

        /// <summary>
        ///     Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        /// <summary>
        ///     Adds an item to this window and shifts all other elements
        /// </summary>
        /// <param name="item">The item to be added</param>
        public void Add(long time, T item) {
            try {
                _listLock.EnterWriteLock();

                _samples++;
                if (Size == Count) {
                    // keep track of what's the last element
                    // so we can reindex on this[ int ]
                    _mostRecentlyRemoved = _list[_tail];
                    _list[_tail] = item;
                    _tail = (_tail + 1) % Size;
                } else {
                    _list.Add(item);
                }
            } finally {
                _listLock.ExitWriteLock();
            }

            Current = (DoubleArray) (object) item;
            Updated?.Invoke(CurrentTime = time, Current);
        }

        /// <summary>
        /// 	 Event handler that fires after this updatable is updated.
        /// </summary>
        public event UpdatedHandler Updated;

        /// <summary>
        ///     Event handler that fires after this updatable is reset.
        /// </summary>
        public event ResettedHandler Resetted;

        /// <summary>
        /// 	 Updates the state of this updatable with the given value and returns true
        /// 	 if this updatable is ready, false otherwise
        /// </summary>
        /// <param name="time"></param>
        /// <param name="input">The value to use to update this updatable</param>
        /// <returns>True if this updatable is ready, false otherwise</returns>
        bool IUpdatable.Update(long time, DoubleArray input) {
            var t = (T) (object) input;
            try {
                _listLock.EnterWriteLock();

                _samples++;
                if (Size == Count) {
                    // keep track of what's the last element
                    // so we can reindex on this[ int ]
                    _mostRecentlyRemoved = _list[_tail];
                    _list[_tail] = t;
                    _tail = (_tail + 1) % Size;
                } else {
                    _list.Add(t);
                }
            } finally {
                _listLock.ExitWriteLock();
            }

            Current = input;
            Updated?.Invoke(CurrentTime = time, Current);
            return IsReady;
        }

        /// <summary>
        ///     Clears this window of all data
        /// </summary>
        public void Reset() {
            try {
                _listLock.EnterWriteLock();

                _samples = 0;
                _list.Clear();
                _tail = 0;
            } finally {
                _listLock.ExitWriteLock();
            }

            Resetted?.Invoke(this);
        }
    }
}