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
    private readonly IDictionary<string, object?> _inner;
    #endregion


    #region Constructors
    /// <summary>
    /// Creates instance.
    /// </summary>
    public BindParameterCollection()
        : this(new Dictionary<string, object?>())
    { }


    /// <summary>
    /// Creates instance.
    /// </summary>
    /// <param name="source"></param>
    public BindParameterCollection(IDictionary<string, object?> source)
        => this._inner = source;
    #endregion


    #region IDictionary<TKey, TValue>
    /// <inheritdoc/>
    public object? this[string key]
    {
        get => this._inner[key];
        set => this._inner[key] = value;
    }


    /// <inheritdoc/>
    ICollection<string> IDictionary<string, object?>.Keys
        => this._inner.Keys;


    /// <inheritdoc/>
    ICollection<object?> IDictionary<string, object?>.Values
        => this._inner.Values;


    /// <inheritdoc/>
    public int Count
        => this._inner.Count;


    /// <inheritdoc/>
    bool ICollection<KeyValuePair<string, object?>>.IsReadOnly
        => this._inner.IsReadOnly;


    /// <inheritdoc/>
    public void Add(string key, object? value)
        => this._inner.Add(key, value);


    /// <inheritdoc/>
    void ICollection<KeyValuePair<string, object?>>.Add(KeyValuePair<string, object?> item)
        => this._inner.Add(item);


    /// <inheritdoc/>
    public void Clear()
        => this._inner.Clear();


    /// <inheritdoc/>
    bool ICollection<KeyValuePair<string, object?>>.Contains(KeyValuePair<string, object?> item)
        => this._inner.Contains(item);


    /// <inheritdoc/>
    public bool ContainsKey(string key)
        => this._inner.ContainsKey(key);


    /// <inheritdoc/>
    void ICollection<KeyValuePair<string, object?>>.CopyTo(KeyValuePair<string, object?>[] array, int arrayIndex)
        => this._inner.CopyTo(array, arrayIndex);


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
    bool ICollection<KeyValuePair<string, object?>>.Remove(KeyValuePair<string, object?> item)
        => this._inner.Remove(item);


    /// <inheritdoc/>
    public bool TryGetValue(string key, out object? value)
        => this._inner.TryGetValue(key, out value);
    #endregion


    #region IReadOnlyDictionary<TKey, TValue>
    /// <inheritdoc/>
    public IEnumerable<string> Keys
        => this._inner.Keys;


    /// <inheritdoc/>
    public IEnumerable<object?> Values
        => this._inner.Values;
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
        var result = new BindParameterCollection();
        var members = TypeAccessor.Create(typeof(T)).GetMembers();
        var accessor = ObjectAccessor.Create(obj);
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
        IDictionary<string, object?> result = new BindParameterCollection();
        foreach (var x in this)
            result.Add(x);
        return (BindParameterCollection)result;
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

