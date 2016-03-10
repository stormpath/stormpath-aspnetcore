// Contains code copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Stormpath.AspNetCore.Internal
{
    /// <summary>
    /// Contains the parsed form values.
    /// </summary>
    public sealed class FormCollection : IEnumerable<KeyValuePair<string, string[]>>
    {
        public static readonly FormCollection Empty = new FormCollection();
#if NETSTANDARD1_3
        private static readonly string[] EmptyKeys = Array.Empty<string>();
        private static readonly string[] EmptyValues = Array.Empty<string>();
#else
        private static readonly string[] EmptyKeys = new string[0];
        private static readonly string[] EmptyValues = new string[0];
#endif 
        private static readonly Enumerator EmptyEnumerator = new Enumerator();
        // Pre-box
        private static readonly IEnumerator<KeyValuePair<string, string[]>> EmptyIEnumeratorType = EmptyEnumerator;
        private static readonly IEnumerator EmptyIEnumerator = EmptyEnumerator;

        private FormCollection()
        {
            // For static Empty
        }

        public FormCollection(IDictionary<string, string[]> fields)
        {
            // can be null
            Store = new Dictionary<string, string[]>(fields);
        }

        private Dictionary<string, string[]> Store { get; set; }

        /// <summary>
        /// Get or sets the associated value from the collection as a single string.
        /// </summary>
        /// <param name="key">The header name.</param>
        /// <returns>the associated value from the collection as a string[] or string[].Empty if the key is not present.</returns>
        public string[] this[string key]
        {
            get
            {
                if (Store == null)
                {
                    return new string[0];
                }

                string[] value;
                if (TryGetValue(key, out value))
                {
                    return value;
                }
                return new string[0];
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:Microsoft.AspNetCore.Http.Internal.HeaderDictionary" />;.
        /// </summary>
        /// <returns>The number of elements contained in the <see cref="T:Microsoft.AspNetCore.Http.Internal.HeaderDictionary" />.</returns>
        public int Count
        {
            get
            {
                return Store?.Count ?? 0;
            }
        }

        public ICollection<string> Keys
        {
            get
            {
                if (Store == null)
                {
                    return EmptyKeys;
                }
                return Store.Keys;
            }
        }

        /// <summary>
        /// Determines whether the <see cref="T:Microsoft.AspNetCore.Http.Internal.HeaderDictionary" /> contains a specific key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>true if the <see cref="T:Microsoft.AspNetCore.Http.Internal.HeaderDictionary" /> contains a specific key; otherwise, false.</returns>
        public bool ContainsKey(string key)
        {
            if (Store == null)
            {
                return false;
            }
            return Store.ContainsKey(key);
        }

        /// <summary>
        /// Retrieves a value from the dictionary.
        /// </summary>
        /// <param name="key">The header name.</param>
        /// <param name="value">The value.</param>
        /// <returns>true if the <see cref="T:Microsoft.AspNetCore.Http.Internal.HeaderDictionary" /> contains the key; otherwise, false.</returns>
        public bool TryGetValue(string key, out string[] value)
        {
            if (Store == null)
            {
                value = default(string[]);
                return false;
            }
            return Store.TryGetValue(key, out value);
        }

        /// <summary>
        /// Returns an struct enumerator that iterates through a collection without boxing and is also used via the <see cref="T:Microsoft.AspNetCore.Http.IFormCollection" /> interface.
        /// </summary>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Http.StructEnumerator" /> object that can be used to iterate through the collection.</returns>
        public Enumerator GetEnumerator()
        {
            if (Store == null || Store.Count == 0)
            {
                // Non-boxed Enumerator
                return EmptyEnumerator;
            }
            // Non-boxed Enumerator
            return new Enumerator(Store.GetEnumerator());
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection, boxes in non-empty path.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator<KeyValuePair<string, string[]>> IEnumerable<KeyValuePair<string, string[]>>.GetEnumerator()
        {
            if (Store == null || Store.Count == 0)
            {
                // Non-boxed Enumerator
                return EmptyIEnumeratorType;
            }
            // Boxed Enumerator
            return Store.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection, boxes in non-empty path.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            if (Store == null || Store.Count == 0)
            {
                // Non-boxed Enumerator
                return EmptyIEnumerator;
            }
            // Boxed Enumerator
            return Store.GetEnumerator();
        }

        public struct Enumerator : IEnumerator<KeyValuePair<string, string[]>>
        {
            // Do NOT make this readonly, or MoveNext will not work
            private Dictionary<string, string[]>.Enumerator _dictionaryEnumerator;
            private bool _notEmpty;

            internal Enumerator(Dictionary<string, string[]>.Enumerator dictionaryEnumerator)
            {
                _dictionaryEnumerator = dictionaryEnumerator;
                _notEmpty = true;
            }

            public bool MoveNext()
            {
                if (_notEmpty)
                {
                    return _dictionaryEnumerator.MoveNext();
                }
                return false;
            }

            public KeyValuePair<string, string[]> Current
            {
                get
                {
                    if (_notEmpty)
                    {
                        return _dictionaryEnumerator.Current;
                    }
                    return default(KeyValuePair<string, string[]>);
                }
            }

            public void Dispose()
            {
            }

            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            void IEnumerator.Reset()
            {
                if (_notEmpty)
                {
                    ((IEnumerator)_dictionaryEnumerator).Reset();
                }
            }
        }
    }
}