﻿using Cysharp.Text;
using QLimitive.Commands;

namespace QLimitive;



/// <summary>
/// Provides query builder.
/// </summary>
/// <typeparam name="T">Table mapping type</typeparam>
public ref struct QueryBuilder<T> //: IDisposable
{
    #region Fields
    private readonly DbDialect dialect;
    private Utf16ValueStringBuilder stringBuilder;
    private BindParameterCollection? bindParameters;
    #endregion


    #region Constructors
    /// <summary>
    /// Creates instance.
    /// </summary>
    /// <param name="dialect"></param>
    /// <remarks>This instance must be call <see cref="Dispose"/> method after <see cref="Build"/>.</remarks>
    public QueryBuilder(DbDialect dialect)
    {
        this.dialect = dialect;
        this.stringBuilder = ZString.CreateStringBuilder();
        this.bindParameters = null;
    }
    #endregion


    #region IDisposable implementations
    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
        => this.stringBuilder.Dispose();
    #endregion


    /// <summary>
    /// Build query up.
    /// </summary>
    /// <returns></returns>
    public Query Build()
    {
        var text = this.stringBuilder.ToString();
        return new Query(text, this.bindParameters);
    }


    #region Commands
    /// <summary>
    /// Builds count statement.
    /// </summary>
    /// <returns></returns>
    public void Count()
    {
        var command = new Count<T>(this.dialect);
        command.Build(ref this.stringBuilder, ref this.bindParameters);
    }
    #endregion
}



/// <summary>
/// Provides shorthand notation for <see cref="QueryBuilder{T}"/>.
/// </summary>
public static class QueryBuilder
{
    #region Count
    /// <summary>
    /// Builds count statement.
    /// </summary>
    /// <typeparam name="T">Table mapping type</typeparam>
    /// <param name="dialect"></param>
    /// <returns></returns>
    public static Query Count<T>(DbDialect dialect)
    {
        using (var builder = new QueryBuilder<T>(dialect))
        {
            builder.Count();
            return builder.Build();
        }
    }
    #endregion
}