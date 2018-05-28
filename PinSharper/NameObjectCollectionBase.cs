#region Imports

using System;
using System.Diagnostics;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

#endregion

public abstract class NameObjectCollectionBase : ICollection
{
    private Dictionary<string, int> _indexByKey;
    private List<Entry> _entries;
    private int? _nullKeyIndex;
    private readonly int _initialCapacity;
    private KeysCollection _keys;

    [ DebuggerDisplay("{ Key = {Key}, Value = {Value} }") ]
    private struct Entry
    {
        public readonly string Key;
        public readonly object Value;

        public Entry(string key, object value)
        {
            Debug.Assert(key != null);

            Key = key;
            Value = value;
        }
    }

    internal IEqualityComparer<string> EqualityComparer { get; private set; }

    protected NameObjectCollectionBase() : 
        this(0) {}

    protected NameObjectCollectionBase(int capacity) : 
        this(capacity, null) {}

    protected NameObjectCollectionBase (IEqualityComparer<string> equalityComparer) : 
        this(0, equalityComparer) {}

    protected NameObjectCollectionBase(int capacity, IEqualityComparer<string> equalityComparer)
    {
        EqualityComparer = equalityComparer ?? StringComparer.CurrentCultureIgnoreCase;
        _initialCapacity = capacity;
        Reset();
    }

    private void Reset()
    {
        _indexByKey = new Dictionary<string, int>(_initialCapacity, EqualityComparer);
        _entries = new List<Entry>(_initialCapacity);
        _nullKeyIndex = null;
    }

    public virtual KeysCollection Keys
    {
        get
        {
            if (_keys == null)
                _keys = new KeysCollection(this);
            return _keys;
        }
    }

    public virtual IEnumerator GetEnumerator()
    {
        return Keys.GetEnumerator();
    }

    public virtual int Count
    {
        get { return _entries.Count; }
    }

    bool ICollection.IsSynchronized
    {
        get { return false; }
    }

    object ICollection.SyncRoot
    {
        get { return this; }
    }

    void ICollection.CopyTo(Array array, int index)
    {
        ((ICollection) Keys).CopyTo(array, index);
    }

    protected bool IsReadOnly { get; set; }

    protected void BaseAdd(string name, object value)
    {
        RequireWriteAccess();

        var entry = new Entry(name, value);
        var index = _entries.Count;

        if (name == null)
        {
            if (_nullKeyIndex == null)
                _nullKeyIndex = index;
        }
        else
        {
            int unused;
            if (!_indexByKey.TryGetValue(name, out unused))
                _indexByKey.Add(name, index);
        }

        _entries.Add(entry);
    }

    protected void BaseClear()
    {
        RequireWriteAccess();
        Reset();
    }

    protected object BaseGet(int index)
    {
        return _entries[index].Value;
    }
   
    protected object BaseGet(string name)
    {
        var index = NameToIndex(name);
        return index == null ? null : _entries[index.Value].Value;
    }

    protected string[] BaseGetAllKeys()
    {
        return _entries.Select(e => e.Key).ToArray();
    }

    protected object[] BaseGetAllValues()
    {
        return _entries.Select((e, i) => BaseGet(i)).ToArray();
    }

    protected object[] BaseGetAllValues(Type type)
    {
        if (type == null)
            throw new ArgumentNullException("type");

        var count = _entries.Count;
        var values = (object[]) Array.CreateInstance(type, count);
        for (var i = 0; i < count; i++)
            values[i] = BaseGet(i);
        return values;
    }

    protected string BaseGetKey(int index)
    {
        return _entries[index].Key;
    }

    protected bool BaseHasKeys()
    {
        return _indexByKey.Count > 0;
    }

    protected void BaseRemove(string name)
    {
        RequireWriteAccess();
        
        if (name != null)
            _indexByKey.Remove(name);
        else
            _nullKeyIndex = null;

        var count = _entries.Count;
        for (var i = 0; i < count; )
        {
            var key = BaseGetKey(i);
            if (EqualityComparer.Equals(key, name))
            {
                _entries.RemoveAt(i);
                count--;
            }
            else
            {
                i++;
            }
        }
    }

    protected void BaseRemoveAt(int index)
    {
        RequireWriteAccess();
        
        var key = BaseGetKey(index);

        if (key != null)
            _indexByKey.Remove(key);
        else
            _nullKeyIndex = null;
        
        _entries.RemoveAt(index);
    }

    protected void BaseSet(int index, object value)
    {
        RequireWriteAccess();
        var current = _entries[index];
        _entries[index] = new Entry(current.Key, value);
    }
   
    protected void BaseSet(string name, object value)
    {
        RequireWriteAccess();

        var index = NameToIndex(name);
        if (index != null)
            _entries[index.Value] = new Entry(name, value);
        else
            BaseAdd(name, value);
    }

    protected void RequireWriteAccess()
    {
        if (IsReadOnly)
            throw new NotSupportedException("Collection is read-only.");
    }

    private int? NameToIndex(string name)
    {
        int index;
        return name == null
             ? _nullKeyIndex
             : _indexByKey.TryGetValue(name, out index)
               ? index
               : (int?) null;
    }

    public class KeysCollection : ICollection
    {
        private readonly NameObjectCollectionBase _collection;

        internal KeysCollection(NameObjectCollectionBase collection)
        {
            _collection = collection;
        }

        public string this[int index] { get { return Get(index); } }
        public int Count { get { return _collection.Count; } }
        bool ICollection.IsSynchronized { get { return false; } }
        object ICollection.SyncRoot { get { return _collection; } }

        public virtual string Get(int index)
        {
            return _collection.BaseGetKey(index);
        }

        public IEnumerator GetEnumerator()
        {
            foreach (var item in _collection._entries)
                yield return item.Key;
        }

        void ICollection.CopyTo(Array array, int arrayIndex)
        {
            _collection._entries.Select(e => e.Key).ToArray().CopyTo(array, arrayIndex);
        }
    }
}
