using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FastMember;

namespace QLimitive;



/// <summary>
/// Represents bind parameters.
/// </summary>
public sealed class BindParameterCollection : IDictionary<string, object?>, IReadOnlyDictionary<string, object?>
{
    #region Fields
    private readonly Dictionary<string, object?> _inner;
    #endregion


    #region Constructors
    /// <summary>
    /// Creates instance.
    /// </summary>
    public BindParameterCollection()
        : this(capacity: 0)
    { }


    /// <summary>
    /// Creates instance.
    /// </summary>
    /// <param name="capacity"></param>
    public BindParameterCollection(int capacity)
        => this._inner = new(capacity);
    #endregion


    #region IDictionary<TKey, TValue>
    /// <inheritdoc/>
    public object? this[string key]
    {
        get => this._inner[key];
        set => this._inner[key] = value;
    }


    /// <inheritdoc/>
    public ICollection<string> Keys
        => this._inner.Keys;


    /// <inheritdoc/>
    public ICollection<object?> Values
        => this._inner.Values;


    /// <inheritdoc/>
    public int Count
        => this._inner.Count;


    /// <inheritdoc/>
    public bool IsReadOnly
        => false;


    /// <inheritdoc/>
    public void Add(string key, object? value)
        => this._inner.Add(key, value);


    /// <inheritdoc/>
    public void Add(KeyValuePair<string, object?> item)
    {
        var dic = (IDictionary<string, object?>)this._inner;
        dic.Add(item);
    }


    /// <inheritdoc/>
    public void Clear()
        => this._inner.Clear();


    /// <inheritdoc/>
    public bool Contains(KeyValuePair<string, object?> item)
    {
        var dic = (IDictionary<string, object?>)this._inner;
        return dic.Contains(item);
    }


    /// <inheritdoc/>
    public bool ContainsKey(string key)
        => this._inner.ContainsKey(key);


    /// <inheritdoc/>
    public void CopyTo(KeyValuePair<string, object?>[] array, int arrayIndex)
    {
        var dic = (IDictionary<string, object?>)this._inner;
        dic.CopyTo(array, arrayIndex);
    }


    /// <inheritdoc/>
    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
        => this._inner.GetEnumerator();


    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
        => this._inner.GetEnumerator();


    /// <inheritdoc/>
    public bool Remove(string key)
        => this._inner.Remove(key);


    /// <inheritdoc/>
    public bool Remove(KeyValuePair<string, object?> item)
    {
        var dic = (IDictionary<string, object?>)this._inner;
        return dic.Remove(item);
    }


    /// <inheritdoc/>
    public bool TryGetValue(string key, [MaybeNullWhen(false)] out object? value)
        => this._inner.TryGetValue(key, out value);
    #endregion


    #region IReadOnlyDictionary<TKey, TValue>
    /// <inheritdoc/>
    IEnumerable<string> IReadOnlyDictionary<string, object?>.Keys
        => this.Keys;


    /// <inheritdoc/>
    IEnumerable<object?> IReadOnlyDictionary<string, object?>.Values
        => this.Values;
    #endregion


    #region Utilities
    /// <summary>
    /// Clones the instance.
    /// </summary>
    /// <returns></returns>
    public BindParameterCollection Clone()
    {
        var result = new BindParameterCollection(this._inner.Count);
        foreach (var x in this)
            result._inner.Add(x.Key, x.Value);
        return result;
    }


    /// <summary>
    /// Overwrites by the specified values.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    public void Overwrite<T>(T obj)
    {
        var members = TypeAccessor.Create(typeof(T)).GetMembers();
        var accessor = ObjectAccessor.Create(obj);
        for (var i = 0; i < members.Count; i++)
        {
            var member = members[i];
            if (!member.CanRead)
                continue;

            if (!this._inner.ContainsKey(member.Name))
                continue;

            this._inner[member.Name] = accessor[member.Name];
        }
    }
    #endregion
}

