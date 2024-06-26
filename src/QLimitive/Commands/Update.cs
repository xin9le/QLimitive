﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cysharp.Text;
using QLimitive.Internals;
using QLimitive.Mappings;

namespace QLimitive.Commands;



/// <summary>
/// Represents update command.
/// </summary>
/// <typeparam name="T"></typeparam>
internal readonly struct Update<T> : IQueryBuildable
{
    #region Properties
    /// <summary>
    /// Gets the database dialect.
    /// </summary>
    private DbDialect Dialect { get; }


    /// <summary>
    /// Gets the members mapped to the column.
    /// </summary>
    private Expression<Func<T, object>>? Members { get; }


    /// <summary>
    /// Gets whether or not to use the ambient value.
    /// </summary>
    private bool UseAmbientValue { get; }
    #endregion


    #region Constructors
    /// <summary>
    /// Creates instance.
    /// </summary>
    public Update(DbDialect dialect, Expression<Func<T, object>>? members, bool useAmbientValue)
    {
        this.Dialect = dialect;
        this.Members = members;
        this.UseAmbientValue = useAmbientValue;
    }
    #endregion


    #region IQueryBuildable implementations
    /// <inheritdoc/>
    public void Build(ref Utf16ValueStringBuilder builder, ref BindParameterCollection? parameters)
    {
        //--- Extract target columns
        HashSet<string>? targetMemberNames = null;
        if (this.Members is not null)
            targetMemberNames = ExpressionHelper.GetMemberNames(this.Members);

        //--- Build SQL
        var table = TableMappingInfo.Get<T>();
        var columns = table.Columns.Span;
        var bracket = this.Dialect.KeywordBracket;
        var prefix = this.Dialect.BindParameterPrefix;

        builder.Append("update ");
        builder.AppendTableName<T>(this.Dialect);
        builder.AppendLine();
        builder.Append("set");
        foreach (var x in columns)
        {
            if (!x.IsMapped)
                continue;

            if (targetMemberNames is null || targetMemberNames.Contains(x.MemberName))
            {
                builder.AppendLine();
                builder.Append("    ");
                builder.Append(bracket.Begin);
                builder.Append(x.ColumnName);
                builder.Append(bracket.End);
                builder.Append(" = ");
                if (this.UseAmbientValue && x.AmbientValue is not null)
                {
                    builder.Append(x.AmbientValue);
                    builder.Append(',');
                }
                else
                {
                    builder.Append(prefix);
                    builder.Append(x.MemberName);
                    builder.Append(',');

                    parameters ??= [];
                    parameters.Add(x.MemberName, null);
                }
            }
        }
        builder.Advance(-1);  // remove last colon
    }
    #endregion
}
