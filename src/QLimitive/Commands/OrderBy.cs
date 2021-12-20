﻿using System;
using System.Linq.Expressions;
using Cysharp.Text;
using QLimitive.Internals;
using QLimitive.Mappings;

namespace QLimitive.Commands;



/// <summary>
/// Represents order by command.
/// </summary>
/// <typeparam name="T"></typeparam>
internal readonly struct OrderBy<T> : IQueryBuildable
{
    #region Properties
    /// <summary>
    /// Gets the database dialect.
    /// </summary>
    private DbDialect Dialect { get; }


    /// <summary>
    /// Gets the expression for the member mapped to the column.
    /// </summary>
    private Expression<Func<T, object?>> Member { get; }


    /// <summary>
    /// Gets whether ascending order.
    /// </summary>
    private bool IsAscending { get; }
    #endregion


    #region Constructors
    /// <summary>
    /// Creates instance.
    /// </summary>
    public OrderBy(DbDialect dialect, Expression<Func<T, object?>> member, bool isAscending)
    {
        this.Dialect = dialect;
        this.Member = member;
        this.IsAscending = isAscending;
    }
    #endregion


    #region IQueryBuildable implementations
    /// <inheritdoc/>
    public void Build(ref Utf16ValueStringBuilder builder, ref BindParameterCollection? parameters)
    {
        var memberName = ExpressionHelper.GetMemberName(this.Member);
        var table = TableMappingInfo.Get<T>();
        var columnName = table.ColumnByMemberName[memberName].ColumnName;
        var bracket = this.Dialect.KeywordBracket;

        builder.AppendLine("order by");
        builder.Append("    ");
        builder.Append(bracket.Begin);
        builder.Append(columnName);
        builder.Append(bracket.End);
        if (!this.IsAscending)
            builder.Append(" desc");
    }
    #endregion
}
