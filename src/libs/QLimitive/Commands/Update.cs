using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
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
    public void Build(ref DefaultInterpolatedStringHandler handler, ref BindParameterCollection? parameters)
    {
        //--- Extract target columns
        HashSet<string>? targetMemberNames = null;
        if (this._members is not null)
            targetMemberNames = ExpressionHelper.GetMemberNames(this._members);

        //--- Build SQL
        var table = TableMappingInfo.Get<T>();
        var columns = table.Columns.AsSpan();
        var bracket = this._dialect.KeywordBracket;
        var prefix = this._dialect.BindParameterPrefix;

        handler.Append("update ");
        handler.AppendTableName<T>(this._dialect);
        handler.AppendLine();
        handler.Append("set");
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
                handler.Append(" = ");
                if (this._useAmbientValue && x.AmbientValue is not null)
                {
                    handler.Append(x.AmbientValue);
                    handler.Append(',');
                }
                else
                {
                    handler.Append(prefix);
                    handler.Append(x.MemberName);
                    handler.Append(',');

                    parameters ??= [];
                    parameters.Add(x.MemberName, null);
                }
            }
        }
        handler.Advance(-1);  // remove last colon
    }
    #endregion
}
