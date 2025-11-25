using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using QLimitive.Internals;
using QLimitive.Mappings;

namespace QLimitive.Commands;



/// <summary>
/// Represents order by command.
/// </summary>
/// <typeparam name="T"></typeparam>
internal readonly struct OrderBy<T>(DbDialect dialect, Expression<Func<T, object>> member, bool isAscending)
    : IQueryBuildable
{
    #region Fields
    private readonly DbDialect _dialect = dialect;
    private readonly Expression<Func<T, object>> _member = member;
    private readonly bool _isAscending = isAscending;
    #endregion


    #region IQueryBuildable
    /// <inheritdoc/>
    public void Build(ref DefaultInterpolatedStringHandler handler, ref BindParameterCollection? parameters)
    {
        var memberName = ExpressionHelper.GetMemberName(this._member);
        if (memberName is null)
            throw new InvalidOperationException();

        var table = TableMappingInfo.Get<T>();
        var columnName = table.ColumnByMemberName[memberName].ColumnName;
        var bracket = this._dialect.KeywordBracket;

        if (handler.Length > 0)
            handler.AppendLine();

        handler.AppendLine("order by");
        handler.Append("    ");
        handler.Append(bracket.Begin);
        handler.Append(columnName);
        handler.Append(bracket.End);
        if (!this._isAscending)
            handler.Append(" desc");
    }
    #endregion
}
