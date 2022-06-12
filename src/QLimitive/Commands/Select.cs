using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cysharp.Text;
using QLimitive.Internals;
using QLimitive.Mappings;

namespace QLimitive.Commands;



/// <summary>
/// Represents select command.
/// </summary>
/// <typeparam name="T"></typeparam>
internal readonly struct Select<T> : IQueryBuildable
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
    #endregion


    #region Constructors
    /// <summary>
    /// Creates instance.
    /// </summary>
    public Select(DbDialect dialect, Expression<Func<T, object>>? members)
    {
        this.Dialect = dialect;
        this.Members = members;
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
        builder.Append("select");
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
                builder.Append(" as ");
                builder.Append(bracket.Begin);
                builder.Append(x.MemberName);
                builder.Append(bracket.End);
                builder.Append(',');
            }
        }
        builder.Advance(-1);  // remove last colon.
        builder.AppendLine();
        builder.Append("from ");
        builder.AppendTableName<T>(this.Dialect);
    }
    #endregion
}
