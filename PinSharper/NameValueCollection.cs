#region Imports

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

public class NameValueCollection : NameObjectCollectionBase
{
    private string[] _cachedKeys;
    private string[] _cachedValues;

    public NameValueCollection() { }

    public NameValueCollection(int capacity) :
        base(capacity) { }

    public NameValueCollection(NameValueCollection col) :
        base(col == null ? null : col.EqualityComparer)
    {
        if (col == null) throw new ArgumentNullException("col");
        Add(col);
    }

    public NameValueCollection(int capacity, NameValueCollection col) : 
        base(capacity, col == null ? null : col.EqualityComparer)
    {
        Add(col);
    }

    public NameValueCollection(IEqualityComparer<string> equalityComparer) :
        base(equalityComparer) { }

    public NameValueCollection(int capacity, IEqualityComparer<string> equalityComparer) :
        base(capacity, equalityComparer) { }

    public virtual string[] AllKeys
    {
        get
        {
            if (_cachedKeys == null)
                _cachedKeys = BaseGetAllKeys();

            return _cachedKeys;
        }
    }

    public string this[int index] { get { return Get(index); } }
    
    public string this[string name]
    {
        get { return Get(name); }
        set { Set(name, value); }
    }

    private List<string> GetStringList(string name)
    {
        return (List<string>) BaseGet(name);
    }

    private List<string> GetStringList(int index)
    {
        return (List<string>) BaseGet(index);
    }

    public void Add(NameValueCollection c)
    {
        if (c == null) throw new ArgumentNullException("c");

        RequireWriteAccess();

        InvalidateCachedArrays();
        var count = c.Count;
        for (var i = 0; i < count; i++)
        {
            var key = c.GetKey(i);
            var those = (IEnumerable<string>) c.BaseGet(i);
            var these = (List<string>) BaseGet(key);
            if (these != null && those != null)
                these.AddRange(those);
            else if (those != null)
                these = new List<string>(those);
            BaseSet(key, these);
        }
    }

    public virtual void Add(string name, string val)
    {
        RequireWriteAccess();

        InvalidateCachedArrays();
        var values = GetStringList(name);
        if (values == null)
        {
            values = new List<string>();
            if (val != null)
                values.Add(val);
            BaseAdd(name, values);
        }
        else
        {
            if (val != null)
                values.Add(val);
        }

    }

    public virtual void Clear()
    {
        RequireWriteAccess();
        InvalidateCachedArrays();
        BaseClear();
    }

    public void CopyTo(Array dest, int index)
    {
        if (dest == null) throw new ArgumentNullException("dest");

        if (_cachedValues == null)
            _cachedValues = Enumerable.Range(0, Count).Select(i => Get(i)).ToArray();

        _cachedValues.CopyTo(dest, index);
    }

    public virtual string Get(int index)
    {
        return ToDelimitedString(GetStringList(index));
    }

    public virtual string Get(string name)
    {
        return ToDelimitedString(GetStringList(name));
    }

    public virtual string GetKey(int index)
    {
        return BaseGetKey(index);
    }

    public virtual string[] GetValues(int index)
    {
        return ToStringArray(GetStringList(index));
    }

    public virtual string[] GetValues(string name)
    {
        return ToStringArray(GetStringList(name));
    }

    public bool HasKeys()
    {
        return BaseHasKeys();
    }

    public virtual void Remove(string name)
    {
        RequireWriteAccess();
        InvalidateCachedArrays();
        BaseRemove(name);
    }

    public virtual void Set(string name, string value)
    {
        RequireWriteAccess(); 
        InvalidateCachedArrays();

        var values = new List<string>();
        if (value != null)
        {
            values.Add(value);
            BaseSet(name, values);
        }
        else
        {
            BaseSet(name, null);
        }
    }

    protected void InvalidateCachedArrays()
    {
        _cachedKeys = null;
        _cachedValues = null;
    }

    private static string ToDelimitedString(ICollection<string> values)
    {
        if (values == null || values.Count == 0)
            return null;

        using (var e = values.GetEnumerator())
        {
            e.MoveNext();
            var sb = new StringBuilder(e.Current);
            while (e.MoveNext())
                sb.Append(',').Append(e.Current);
            return sb.ToString();
        }
    }

    private static string[] ToStringArray(ICollection<string> values)
    {
        return values == null || values.Count == 0 ? null : values.ToArray();
    }
}
