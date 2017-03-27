using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.LazyLoading.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.EntityFrameworkCore.LazyLoading
{
    public sealed class LazyCollection<T> : IList<T>
        where T : class
    {
        private bool _loaded;
        private bool _loading;
        private readonly DbContext _ctx;
        private readonly string _collectionName;
        private readonly object _parent;
        private readonly IList<T> _entries = new List<T>();

        public LazyCollection(DbContext ctx, object parent, string collectionName)
        {
            _ctx = ctx;
            _parent = parent;
            _collectionName = collectionName;
        }

        private void EnsureLoaded()
        {
            if (_loaded == false)
            {
                if (_loading == true)
                {
                    return;
                }

                _loading = true;

                var concurrencyDetector = _ctx.GetService<EntityFrameworkCore.Internal.IConcurrencyDetector>() as IConcurrencyDetector;
                if (concurrencyDetector == null)
                {
                    throw new LazyLoadingConfigurationException($"Service of type '{typeof(EntityFrameworkCore.Internal.IConcurrencyDetector).FullName}' must be replaced by a service of type '{typeof(IConcurrencyDetector).FullName}' in order to use LazyLoading");
                }

                if (concurrencyDetector.IsInOperation())
                {
                    _loading = false;
                    return;
                }

                var entries = _ctx
                    .Entry(_parent)
                    .Collection(_collectionName)
                    .Query()
                    .OfType<T>()
                    .ToList();

                /*if (typeof(ILazyLoading).IsAssignableFrom(typeof(T)))
                {
                    foreach (var entry in entries)
                    {
                        ((ILazyLoading)entry).UseLazyLoading(_ctx);
                    }
                }*/

                _entries.Clear();

                foreach (var entry in entries)
                {
                    _entries.Add(entry);
                }

                _loaded = true;
                _loading = false;
            }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            EnsureLoaded();

            return _entries.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (this as ICollection<T>).GetEnumerator();
        }

        int ICollection<T>.Count
        {
            get
            {
                EnsureLoaded();
                return _entries.Count;
            }
        }

        bool ICollection<T>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        void ICollection<T>.Add(T item)
        {
            EnsureLoaded();
            _entries.Add(item);
        }

        void ICollection<T>.Clear()
        {
            EnsureLoaded();
            _entries.Clear();
        }

        bool ICollection<T>.Contains(T item)
        {
            EnsureLoaded();
            return _entries.Contains(item);
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            EnsureLoaded();
            _entries.CopyTo(array, arrayIndex);
        }

        bool ICollection<T>.Remove(T item)
        {
            EnsureLoaded();
            return _entries.Remove(item);
        }

        T IList<T>.this[int index]
        {
            get
            {
                EnsureLoaded();
                return _entries[index];
            }

            set
            {
                EnsureLoaded();
                _entries[index] = value;
            }
        }

        int IList<T>.IndexOf(T item)
        {
            EnsureLoaded();
            return _entries.IndexOf(item);
        }

        void IList<T>.Insert(int index, T item)
        {
            EnsureLoaded();
            _entries.Insert(index, item);
        }

        void IList<T>.RemoveAt(int index)
        {
            EnsureLoaded();
            _entries.RemoveAt(index);
        }

        public override string ToString()
        {
            EnsureLoaded();
            return _entries.ToString();
        }

        public override int GetHashCode()
        {
            EnsureLoaded();
            return _entries.GetHashCode();
        }
    }
}
