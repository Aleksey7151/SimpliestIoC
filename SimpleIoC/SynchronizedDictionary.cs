using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SimpleIoC
{
    // the basis of this class is taked from MSDN ReaderWriterLockSlim Class. 
    internal class SynchronizedDictionary<TKey, TValue> : IDisposable where TKey : class where TValue : class
    {
        private ReaderWriterLockSlim _cacheLock = new ReaderWriterLockSlim();
        private readonly Dictionary<TKey, TValue> _innerCache = new Dictionary<TKey, TValue>();

        public int Count => _innerCache.Count;

        public TValue Read(TKey key)
        {
            _cacheLock.EnterReadLock();
            try
            {
                return _innerCache[key];
            }
            finally
            {
                _cacheLock.ExitReadLock();
            }
        }

        public bool ContainsKey(TKey key)
        {
            _cacheLock.EnterReadLock();
            try
            {
                return _innerCache.ContainsKey(key);
            }
            finally
            {
                _cacheLock.ExitReadLock();
            }
        }

        public void Add(TKey key, TValue value)
        {
            _cacheLock.EnterWriteLock();
            try
            {
                _innerCache.Add(key, value);
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }
        }

        public bool AddWithTimeout(TKey key, TValue value, int timeout)
        {
            if (_cacheLock.TryEnterWriteLock(timeout))
            {
                try
                {
                    _innerCache.Add(key, value);
                }
                finally
                {
                    _cacheLock.ExitWriteLock();
                }
                return true;
            }
            return false;
        }

        public AddOrUpdateStatus AddOrUpdate(TKey key, TValue value)
        {
            _cacheLock.EnterUpgradeableReadLock();
            try
            {
                TValue result;
                if (_innerCache.TryGetValue(key, out result))
                {
                    if (result == value)
                    {
                        return AddOrUpdateStatus.Unchanged;
                    }
                    else
                    {
                        _cacheLock.EnterWriteLock();
                        try
                        {
                            _innerCache[key] = value;
                        }
                        finally
                        {
                            _cacheLock.ExitWriteLock();
                        }
                        return AddOrUpdateStatus.Updated;
                    }
                }
                else
                {
                    _cacheLock.EnterWriteLock();
                    try
                    {
                        _innerCache.Add(key, value);
                    }
                    finally
                    {
                        _cacheLock.ExitWriteLock();
                    }
                    return AddOrUpdateStatus.Added;
                }
            }
            finally
            {
                _cacheLock.ExitUpgradeableReadLock();
            }
        }

        public void Delete(TKey key)
        {
            _cacheLock.EnterWriteLock();
            try
            {
                _innerCache.Remove(key);
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }
        }

        public enum AddOrUpdateStatus
        {
            Added,
            Updated,
            Unchanged
        }

        public void Dispose()
        {
            if (_cacheLock == null) return;

            _cacheLock.EnterWriteLock();
            try
            {
                var services = _innerCache.Values.ToArray();
                foreach (var service in services)
                {
                    var disposable = service as IDisposable;
                    disposable?.Dispose();
                }
                _innerCache.Clear();
            }
            finally
            {
                _cacheLock.ExitWriteLock();
                _cacheLock?.Dispose();
                _cacheLock = null;
            }
        }


        ~SynchronizedDictionary()
        {
            if (_cacheLock == null) return;

            _cacheLock.EnterWriteLock();
            try
            {
                var services = _innerCache.Values.ToArray();
                foreach (var service in services)
                {
                    var disposable = service as IDisposable;
                    disposable?.Dispose();
                }
                _innerCache.Clear();
            }
            finally
            {
                _cacheLock.ExitWriteLock();
                _cacheLock?.Dispose();
                _cacheLock = null;
            }
        }
    }
}
