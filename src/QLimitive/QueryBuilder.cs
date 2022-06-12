using System;
using System.Linq.Expressions;
using Cysharp.Text;
using QLimitive.Commands;

namespace QLimitive;



/// <summary>
/// Provides query builder.
/// </summary>
/// <typeparam name="T">Table mapping type</typeparam>
public ref struct QueryBuilder<T> //: IDisposable
{
    #region Fields
    private readonly DbDialect _dialect;
    private Utf16ValueStringBuilder _stringBuilder;
    private BindParameterCollection? _bindParameters;
    #endregion


    #region Constructors
    /// <summary>
    /// Creates instance.
    /// </summary>
    /// <param name="dialect"></param>
    /// <remarks>This instance must be call <see cref="Dispose"/> method after <see cref="Build"/>.</remarks>
    public QueryBuilder(DbDialect dialect)
    {
        this._dialect = dialect;
        this._stringBuilder = ZString.CreateStringBuilder();
        this._bindParameters = null;
    }
    #endregion


    #region IDisposable implementations
    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
        => this._stringBuilder.Dispose();
    #endregion


    /// <summary>
    /// Build query up.
    /// </summary>
    /// <returns></returns>
    public Query Build()
    {
        var text = this._stringBuilder.ToString();
        return new Query(text, this._bindParameters);
    }


    #region Commands
    /// <summary>
    /// Builds count statement.
    /// </summary>
    /// <returns></returns>
    public void Count()
    {
        var command = new Count<T>(this._dialect);
        command.Build(ref this._stringBuilder, ref this._bindParameters);
    }


    /// <summary>
    /// Builds select statement.
    /// </summary>
    /// <param name="members">Members that mapped to the target column. If null, all columns are targeted.</param>
    /// <returns></returns>
    public void Select(Expression<Func<T, object>>? members = null)
    {
        var command = new Select<T>(this._dialect, members);
        command.Build(ref this._stringBuilder, ref this._bindParameters);
    }


    /// <summary>
    /// Builds update statement.
    /// </summary>
    /// <param name="members">Members that mapped to the target column. If null, all columns are targeted.</param>
    /// <param name="useAmbientValue"></param>
    /// <returns></returns>
    public void Update(Expression<Func<T, object>>? members = null, bool useAmbientValue = false)
    {
        var command = new Update<T>(this._dialect, members, useAmbientValue);
        command.Build(ref this._stringBuilder, ref this._bindParameters);
    }


    /// <summary>
    /// Builds insert statement.
    /// </summary>
    /// <param name="useAmbientValue"></param>
    /// <returns></returns>
    public void Insert(bool useAmbientValue = false)
    {
        var command = new Insert<T>(this._dialect, useAmbientValue);
        command.Build(ref this._stringBuilder, ref this._bindParameters);
    }


    /// <summary>
    /// Builds delete statement.
    /// </summary>
    /// <returns></returns>
    public void Delete()
    {
        var command = new Delete<T>(this._dialect);
        command.Build(ref this._stringBuilder, ref this._bindParameters);
    }


    /// <summary>
    /// Builds truncate statement.
    /// </summary>
    /// <returns></returns>
    public void Truncate()
    {
        var command = new Truncate<T>(this._dialect);
        command.Build(ref this._stringBuilder, ref this._bindParameters);
    }


    /// <summary>
    /// Builds where clause.
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public void Where(Expression<Func<T, bool>> predicate)
    {
        var command = new Where<T>(this._dialect, predicate);
        command.Build(ref this._stringBuilder, ref this._bindParameters);
    }


    /// <summary>
    /// Builds ascending order-by clause.
    /// </summary>
    /// <param name="member"></param>
    /// <returns></returns>
    public void OrderBy(Expression<Func<T, object>> member)
    {
        var command = new OrderBy<T>(this._dialect, member, true);
        command.Build(ref this._stringBuilder, ref this._bindParameters);
    }


    /// <summary>
    /// Builds descending order-by clause.
    /// </summary>
    /// <param name="member"></param>
    /// <returns></returns>
    public void OrderByDescending(Expression<Func<T, object>> member)
    {
        var command = new OrderBy<T>(this._dialect, member, false);
        command.Build(ref this._stringBuilder, ref this._bindParameters);
    }


    /// <summary>
    /// Builds ascending then-by clause.
    /// </summary>
    /// <param name="member"></param>
    /// <returns></returns>
    public void ThenBy(Expression<Func<T, object>> member)
    {
        var command = new ThenBy<T>(this._dialect, member, true);
        command.Build(ref this._stringBuilder, ref this._bindParameters);
    }


    /// <summary>
    /// Builds descending then-by clause.
    /// </summary>
    /// <param name="member"></param>
    /// <returns></returns>
    public void ThenByDescending(Expression<Func<T, object>> member)
    {
        var command = new ThenBy<T>(this._dialect, member, false);
        command.Build(ref this._stringBuilder, ref this._bindParameters);
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


    #region Select
    /// <summary>
    /// Builds select statement.
    /// </summary>
    /// <typeparam name="T">Table mapping type</typeparam>
    /// <param name="dialect"></param>
    /// <param name="members">Members that mapped to the target column. If null, all columns are targeted.</param>
    /// <returns></returns>
    public static Query Select<T>(DbDialect dialect, Expression<Func<T, object>>? members = null)
    {
        using (var builder = new QueryBuilder<T>(dialect))
        {
            builder.Select(members);
            return builder.Build();
        }
    }
    #endregion


    #region Update
    /// <summary>
    /// Builds update statement.
    /// </summary>
    /// <typeparam name="T">Table mapping type</typeparam>
    /// <param name="dialect"></param>
    /// <param name="members">Members that mapped to the target column. If null, all columns are targeted.</param>
    /// <param name="useAmbientValue"></param>
    /// <returns></returns>
    public static Query Update<T>(DbDialect dialect, Expression<Func<T, object>>? members = null, bool useAmbientValue = false)
    {
        using (var builder = new QueryBuilder<T>(dialect))
        {
            builder.Update(members, useAmbientValue);
            return builder.Build();
        }
    }
    #endregion


    #region Insert
    /// <summary>
    /// Builds insert statement.
    /// </summary>
    /// <typeparam name="T">Table mapping type</typeparam>
    /// <param name="dialect"></param>
    /// <param name="useAmbientValue"></param>
    /// <returns></returns>
    public static Query Insert<T>(DbDialect dialect, bool useAmbientValue = false)
    {
        using (var builder = new QueryBuilder<T>(dialect))
        {
            builder.Insert(useAmbientValue);
            return builder.Build();
        }
    }
    #endregion


    #region Delete
    /// <summary>
    /// Builds delete statement.
    /// </summary>
    /// <typeparam name="T">Table mapping type</typeparam>
    /// <param name="dialect"></param>
    /// <returns></returns>
    public static Query Delete<T>(DbDialect dialect)
    {
        using (var builder = new QueryBuilder<T>(dialect))
        {
            builder.Delete();
            return builder.Build();
        }
    }
    #endregion


    #region Truncate
    /// <summary>
    /// Builds truncate statement.
    /// </summary>
    /// <typeparam name="T">Table mapping type</typeparam>
    /// <param name="dialect"></param>
    /// <returns></returns>
    public static Query Truncate<T>(DbDialect dialect)
    {
        using (var builder = new QueryBuilder<T>(dialect))
        {
            builder.Truncate();
            return builder.Build();
        }
    }
    #endregion
}
