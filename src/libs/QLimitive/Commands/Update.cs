using System;
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
internal readonly struct Update<T>(DbDialect dialect, Expression<Func<T, object>>? members, bool useAmbientValue)
    : IQueryBuildable
{
    #region Fields
    private readonly DbDialect _dialect = dialect;
    private readonly Expression<Func<T, object>>? _members = members;
    private readonly bool _useAmbientValue = useAmbientValue;
    #endregion


    #region IQueryBuildable
    /// <inheritdoc/>
    public void Build(ref Utf16ValueStringBuilder builder, ref BindParameterCollection? parameters)
    {
        //--- Extract target columns
        HashSet<string>? targetMemberNames = null;
        if (this._members is not null)
            targetMemberNames = ExpressionHelper.GetMemberNames(this._members);

        //--- Build SQL
        var table = TableMappingInfo.Get<T>();
        var columns = table.Columns.Span;
        var bracket = this._dialect.KeywordBracket;
        var prefix = this._dialect.BindParameterPrefix;

        builder.Append("update ");
        builder.AppendTableName<T>(this._dialect);
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
                if (this._useAmbientValue && x.AmbientValue is not null)
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
