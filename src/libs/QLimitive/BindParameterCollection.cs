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
    #region Properties
    /// <summary>
    /// Gets the key/value store that held inside.
    /// </summary>
    private IDictionary<string, object?> Inner { get; }
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
        => this.Inner = source;
    #endregion


    #region IDictionary<TKey, TValue>
    /// <inheritdoc/>
    public object? this[string key]
    {
        get => this.Inner[key];
        set => this.Inner[key] = value;
    }


    /// <inheritdoc/>
    ICollection<string> IDictionary<string, object?>.Keys
        => this.Inner.Keys;


    /// <inheritdoc/>
    ICollection<object?> IDictionary<string, object?>.Values
        => this.Inner.Values;


    /// <inheritdoc/>
    public int Count
        => this.Inner.Count;


    /// <inheritdoc/>
    bool ICollection<KeyValuePair<string, object?>>.IsReadOnly
        => this.Inner.IsReadOnly;


    /// <inheritdoc/>
    public void Add(string key, object? value)
        => this.Inner.Add(key, value);


    /// <inheritdoc/>
    void ICollection<KeyValuePair<string, object?>>.Add(KeyValuePair<string, object?> item)
        => this.Inner.Add(item);


    /// <inheritdoc/>
    public void Clear()
        => this.Inner.Clear();


    /// <inheritdoc/>
    bool ICollection<KeyValuePair<string, object?>>.Contains(KeyValuePair<string, object?> item)
        => this.Inner.Contains(item);


    /// <inheritdoc/>
    public bool ContainsKey(string key)
        => this.Inner.ContainsKey(key);


    /// <inheritdoc/>
    void ICollection<KeyValuePair<string, object?>>.CopyTo(KeyValuePair<string, object?>[] array, int arrayIndex)
        => this.Inner.CopyTo(array, arrayIndex);


    /// <inheritdoc/>
    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
        => this.Inner.GetEnumerator();


    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
        => this.Inner.GetEnumerator();


    /// <inheritdoc/>
    public bool Remove(string key)
        => this.Inner.Remove(key);


    /// <inheritdoc/>
    bool ICollection<KeyValuePair<string, object?>>.Remove(KeyValuePair<string, object?> item)
        => this.Inner.Remove(item);


    /// <inheritdoc/>
    public bool TryGetValue(string key, out object? value)
        => this.Inner.TryGetValue(key, out value);
    #endregion


    #region IReadOnlyDictionary<TKey, TValue>
    /// <inheritdoc/>
    public IEnumerable<string> Keys
        => this.Inner.Keys;


    /// <inheritdoc/>
    public IEnumerable<object?> Values
        => this.Inner.Values;
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

            if (!this.Inner.ContainsKey(member.Name))
                continue;

            this.Inner[member.Name] = accessor[member.Name];
        }
    }
    #endregion
}

