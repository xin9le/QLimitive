using System;
using System.Linq.Expressions;
using Cysharp.Text;
using QLimitive.Internals;
using QLimitive.Mappings;

namespace QLimitive.Commands;



/// <summary>
/// Represents order by command.
/// </summary>
/// <typeparam name="T"></typeparam>
internal readonly struct ThenBy<T>(DbDialect dialect, Expression<Func<T, object>> member, bool isAscending)
    : IQueryBuildable
{
    #region Fields
    private readonly DbDialect _dialect = dialect;
    private readonly Expression<Func<T, object>> _member = member;
    private readonly bool _isAscending = isAscending;
    #endregion


    #region IQueryBuildable
    /// <inheritdoc/>
    public void Build(ref Utf16ValueStringBuilder builder, ref BindParameterCollection? parameters)
    {
        var memberName = ExpressionHelper.GetMemberName(this._member);
        if (memberName is null)
            throw new InvalidOperationException();

        var table = TableMappingInfo.Get<T>();
        var columnName = table.ColumnByMemberName[memberName].ColumnName;
        var bracket = this._dialect.KeywordBracket;

        if (builder.Length > 0)
            builder.AppendLine(',');

        builder.Append("    ");
        builder.Append(bracket.Begin);
        builder.Append(columnName);
        builder.Append(bracket.End);
        if (!this._isAscending)
            builder.Append(" desc");
    }
    #endregion
}
