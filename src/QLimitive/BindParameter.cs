﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using FastMember;
using QLimitive.Internals;

namespace QLimitive;



/// <summary>
/// Represents bind parameters.
/// </summary>
public sealed class BindParameter : IDictionary<string, object?>, IReadOnlyDictionary<string, object?>
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
    public BindParameter()
        : this(new Dictionary<string, object?>())
    { }


    /// <summary>
    /// Creates instance.
    /// </summary>
    /// <param name="source"></param>
    public BindParameter(IDictionary<string, object?> source)
        => this.Inner = source;
    #endregion


    #region IDictionary<TKey, TValue> implementations
    /// <summary>
    /// Gets or sets the element with the specified key.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public object? this[string key]
    {
        get => this.Inner[key];
        set => this.Inner[key] = value;
    }


    /// <summary>
    /// Gets an <see cref="ICollection{T}"/> containing the keys of the <see cref="IDictionary{TKey, TValue}"/>.
    /// </summary>
    ICollection<string> IDictionary<string, object?>.Keys
        => this.Inner.Keys;


    /// <summary>
    /// Gets an <see cref="ICollection{T}"/> containing the values of the <see cref="IDictionary{TKey, TValue}"/>.
    /// </summary>
    ICollection<object?> IDictionary<string, object?>.Values
        => this.Inner.Values;


    /// <summary>
    /// Gets the number of elements contained in the <see cref="ICollection{T}"/>.
    /// </summary>
    public int Count
        => this.Inner.Count;


    /// <summary>
    /// Gets a value indicating whether the <see cref="ICollection{T}"/> is read-only.
    /// </summary>
    bool ICollection<KeyValuePair<string, object?>>.IsReadOnly
        => this.Inner.IsReadOnly;


    /// <summary>
    /// Adds an item to the <see cref="ICollection{T}"/>.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void Add(string key, object? value)
        => this.Inner.Add(key, value);


    /// <summary>
    /// Adds an item to the <see cref="ICollection{T}"/>.
    /// </summary>
    /// <param name="item"></param>
    void ICollection<KeyValuePair<string, object?>>.Add(KeyValuePair<string, object?> item)
        => this.Inner.Add(item);


    /// <summary>
    /// Removes all items from the <see cref="ICollection{T}"/>.
    /// </summary>
    public void Clear()
        => this.Inner.Clear();


    /// <summary>
    /// Determines whether the <see cref="ICollection{T}"/> contains a specific value.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    bool ICollection<KeyValuePair<string, object?>>.Contains(KeyValuePair<string, object?> item)
        => this.Inner.Contains(item);


    /// <summary>
    /// Determines whether the <see cref="IDictionary{TKey, TValue}"/> contains an element with the specified key.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool ContainsKey(string key)
        => this.Inner.ContainsKey(key);


    /// <summary>
    /// Copies the elements of the <see cref="ICollection{T}"/> to an <see cref="Array"/>, starting at a particular <seealso cref="Array"/> index.
    /// </summary>
    /// <param name="array"></param>
    /// <param name="arrayIndex"></param>
    void ICollection<KeyValuePair<string, object?>>.CopyTo(KeyValuePair<string, object?>[] array, int arrayIndex)
        => this.Inner.CopyTo(array, arrayIndex);


    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns></returns>
    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
        => this.Inner.GetEnumerator();


    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns></returns>
    IEnumerator IEnumerable.GetEnumerator()
        => this.Inner.GetEnumerator();


    /// <summary>
    /// Removes the first occurrence of a specific object from the <see cref="ICollection{T}"/>.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool Remove(string key)
        => this.Inner.Remove(key);


    /// <summary>
    /// Removes the first occurrence of a specific object from the <see cref="ICollection{T}"/>.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    bool ICollection<KeyValuePair<string, object?>>.Remove(KeyValuePair<string, object?> item)
        => this.Inner.Remove(item);


    /// <summary>
    /// Gets the value associated with the specified key.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool TryGetValue(string key, out object? value)
        => this.Inner.TryGetValue(key, out value);
    #endregion


    #region IReadOnlyDictionary<TKey, TValue> implementations
    /// <summary>
    /// Gets an enumerable collection that contains the keys in the read-only dictionary.
    /// </summary>
    public IEnumerable<string> Keys
        => this.Inner.Keys;


    /// <summary>
    /// Gets an enumerable collection that contains the values in the read-only dictionary.
    /// </summary>
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
    public static BindParameter From<T>(T obj)
    {
        var result = new BindParameter();
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
    public BindParameter Clone()
    {
        IDictionary<string, object?> result = new BindParameter();
        foreach (var x in this)
            result.Add(x);
        return (BindParameter)result;
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
    public void Append<T>(T obj, Expression<Func<T, object?>> targetProperties)
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

