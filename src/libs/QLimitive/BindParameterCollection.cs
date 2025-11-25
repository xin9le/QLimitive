using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using FastMember;
using QLimitive.Internals;

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


    /// <summary>
    /// Gets the number of key/value pairs.
    /// </summary>
    public int Count
        => this._inner.Count;


    /// <summary>
    /// Gets or sets the value associated with the specified key.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public object? this[string key]
    {
        get => this._inner[key];
        set => this._inner[key] = value;
    }


    /// <summary>
    /// Adds the specified key and value.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void Add(string key, object? value)
        => this._inner.Add(key, value);


    /// <summary>
    /// Determines whether the collection contains the specified key.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool ContainsKey(string key)
        => this._inner.ContainsKey(key);


    /// <summary>
    /// Removes the value with the specified key.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool Remove(string key)
        => this._inner.Remove(key);


    /// <summary>
    /// Gets the value associated with the specified key.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool TryGetValue(string key, out object? value)
        => this._inner.TryGetValue(key, out value);


    /// <summary>
    /// Removes all keys and values.
    /// </summary>
    public void Clear()
        => this._inner.Clear();


    #region IDictionary<TKey, TValue>
    /// <inheritdoc/>
    ICollection<string> IDictionary<string, object?>.Keys
        => this._inner.Keys;


    /// <inheritdoc/>
    ICollection<object?> IDictionary<string, object?>.Values
        => this._inner.Values;


    /// <inheritdoc/>
    int ICollection<KeyValuePair<string, object?>>.Count
        => this._inner.Count;


    /// <inheritdoc/>
    bool ICollection<KeyValuePair<string, object?>>.IsReadOnly
        => false;


    /// <inheritdoc/>
    object? IDictionary<string, object?>.this[string key]
    {
        get => this._inner[key];
        set => this._inner[key] = value;
    }


    /// <inheritdoc/>
    void IDictionary<string, object?>.Add(string key, object? value)
        => this._inner.Add(key, value);


    /// <inheritdoc/>
    bool IDictionary<string, object?>.ContainsKey(string key)
        => this._inner.ContainsKey(key);


    /// <inheritdoc/>
    bool IDictionary<string, object?>.Remove(string key)
        => this._inner.Remove(key);


    /// <inheritdoc/>
    bool IDictionary<string, object?>.TryGetValue(string key, out object? value)
        => this._inner.TryGetValue(key, out value);


    /// <inheritdoc/>
    void ICollection<KeyValuePair<string, object?>>.Add(KeyValuePair<string, object?> item)
    {
        var dic = (IDictionary<string, object?>)this._inner;
        dic.Add(item);
    }


    /// <inheritdoc/>
    void ICollection<KeyValuePair<string, object?>>.Clear()
        => this._inner.Clear();


    /// <inheritdoc/>
    bool ICollection<KeyValuePair<string, object?>>.Contains(KeyValuePair<string, object?> item)
    {
        var dic = (IDictionary<string, object?>)this._inner;
        return dic.Contains(item);
    }


    /// <inheritdoc/>
    void ICollection<KeyValuePair<string, object?>>.CopyTo(KeyValuePair<string, object?>[] array, int arrayIndex)
    {
        var dic = (IDictionary<string, object?>)this._inner;
        dic.CopyTo(array, arrayIndex);
    }


    /// <inheritdoc/>
    bool ICollection<KeyValuePair<string, object?>>.Remove(KeyValuePair<string, object?> item)
    {
        var dic = (IDictionary<string, object?>)this._inner;
        return dic.Remove(item);
    }


    /// <inheritdoc/>
    IEnumerator<KeyValuePair<string, object?>> IEnumerable<KeyValuePair<string, object?>>.GetEnumerator()
        => this._inner.GetEnumerator();


    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
        => this._inner.GetEnumerator();
    #endregion


    #region IReadOnlyDictionary<TKey, TValue>
    /// <inheritdoc/>
    IEnumerable<string> IReadOnlyDictionary<string, object?>.Keys
        => this._inner.Keys;


    /// <inheritdoc/>
    IEnumerable<object?> IReadOnlyDictionary<string, object?>.Values
        => this._inner.Values;


    /// <inheritdoc/>
    int IReadOnlyCollection<KeyValuePair<string, object?>>.Count
        => this._inner.Count;


    /// <inheritdoc/>
    object? IReadOnlyDictionary<string, object?>.this[string key]
        => this._inner[key];


    /// <inheritdoc/>
    bool IReadOnlyDictionary<string, object?>.ContainsKey(string key)
        => this._inner.ContainsKey(key);


    /// <inheritdoc/>
    bool IReadOnlyDictionary<string, object?>.TryGetValue(string key, out object? value)
        => this._inner.TryGetValue(key, out value);
    #endregion


    #region Create
    /// <summary>
    /// Creates an instance from the specified object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static BindParameterCollection From<T>(T obj)
    {
        var members = TypeAccessor.Create(typeof(T)).GetMembers();
        var accessor = ObjectAccessor.Create(obj);
        var result = new BindParameterCollection(capacity: members.Count);
        for (var i = 0; i < members.Count; i++)
        {
            var member = members[i];
            if (member.CanRead)
                result.Add(member.Name, accessor[member.Name]);
        }
        return result;
    }


    /// <summary>
    /// Clones the instance.
    /// </summary>
    /// <returns></returns>
    public BindParameterCollection Clone()
    {
        var result = new BindParameterCollection(this._inner.Count);
        foreach (var x in this)
            result.Add(x.Key, x.Value);
        return result;
    }
    #endregion


    #region Append
    /// <summary>
    /// Appends the specified values.
    /// </summary>
    /// <param name="kvs"></param>
    public void Append(IEnumerable<KeyValuePair<string, object?>> kvs)
    {
        foreach (var x in kvs)
            this.Add(x.Key, x.Value);
    }


    /// <summary>
    /// Appends the specified values.
    /// </summary>
    /// <param name="obj"></param>
    public void Append<T>(T obj)
    {
        var members = TypeAccessor.Create(typeof(T)).GetMembers();
        var accessor = ObjectAccessor.Create(obj);
        for (var i = 0; i < members.Count; i++)
        {
            var member = members[i];
            if (member.CanRead)
                this.Add(member.Name, accessor[member.Name]);
        }
    }


    /// <summary>
    /// Appends the specified values.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="targetProperties"></param>
    public void Append<T>(T obj, Expression<Func<T, object>> targetProperties)
    {
        var memberNames = ExpressionHelper.GetMemberNames(targetProperties);
        var members = TypeAccessor.Create(typeof(T)).GetMembers();
        var accessor = ObjectAccessor.Create(obj);
        for (var i = 0; i < members.Count; i++)
        {
            var member = members[i];
            if (!member.CanRead)
                continue;
            
            if (!memberNames.Contains(member.Name))
                continue;

            this.Add(member.Name, accessor[member.Name]);
        }
    }
    #endregion


    #region Overwrite
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

