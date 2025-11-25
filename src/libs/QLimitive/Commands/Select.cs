using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using QLimitive.Internals;
using QLimitive.Mappings;

namespace QLimitive.Commands;



/// <summary>
/// Represents select command.
/// </summary>
/// <typeparam name="T"></typeparam>
internal readonly struct Select<T>(DbDialect dialect, Expression<Func<T, object>>? members)
    : IQueryBuildable
{
    #region Fields
    private readonly DbDialect _dialect = dialect;
    private readonly Expression<Func<T, object>>? _members = members;
    #endregion


    #region IQueryBuildable
    /// <inheritdoc/>
    public void Build(ref DefaultInterpolatedStringHandler handler, ref BindParameterCollection? parameters)
    {
        //--- Extract target columns
        HashSet<string>? targetMemberNames = null;
        if (this._members is not null)
            targetMemberNames = ExpressionHelper.GetMemberNames(this._members);

        //--- Build SQL
        var table = TableMappingInfo.Get<T>();
        var columns = table.Columns.Span;
        var bracket = this._dialect.KeywordBracket;
        handler.Append("select");
        foreach (var x in columns)
        {
            if (!x.IsMapped)
                continue;

            if (targetMemberNames is null || targetMemberNames.Contains(x.MemberName))
            {
                handler.AppendLine();
                handler.Append("    ");
                handler.Append(bracket.Begin);
                handler.Append(x.ColumnName);
                handler.Append(bracket.End);
                handler.Append(" as ");
                handler.Append(bracket.Begin);
                handler.Append(x.MemberName);
                handler.Append(bracket.End);
                handler.Append(',');
            }
        }
        handler.Advance(-1);  // remove last colon.
        handler.AppendLine();
        handler.Append("from ");
        handler.AppendTableName<T>(this._dialect);
    }
    #endregion
}
