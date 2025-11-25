using System.Runtime.CompilerServices;
using QLimitive.Internals;
using QLimitive.Mappings;

namespace QLimitive.Commands;



/// <summary>
/// Represents insert command.
/// </summary>
/// <typeparam name="T"></typeparam>
internal readonly struct Insert<T>(DbDialect dialect, bool useAmbientValue)
    : IQueryBuildable
{
    #region Fields
    private readonly DbDialect _dialect = dialect;
    private readonly bool _useAmbientValue = useAmbientValue;
    #endregion


    #region IQueryBuildable
    /// <inheritdoc/>
    public void Build(ref DefaultInterpolatedStringHandler handler, ref BindParameterCollection? parameters)
    {
        var table = TableMappingInfo.Get<T>();
        var columns = table.Columns.AsSpan();
        var bracket = this._dialect.KeywordBracket;
        var prefix = this._dialect.BindParameterPrefix;

        handler.Append("insert into ");
        handler.AppendTableName<T>(this._dialect);
        handler.AppendLine();
        handler.Append("(");
        foreach (var x in columns)
        {
            if (!x.IsMapped)
                continue;

            if (x.IsAutoIncrement)
                continue;

            handler.AppendLine();
            handler.AppendLiteral("    ");
            handler.AppendFormatted(bracket.Begin);
            handler.AppendFormatted(x.ColumnName);
            handler.AppendFormatted(bracket.End);
            handler.AppendLiteral(",");
        }
        handler.Advance(-1);
        handler.AppendLine();
        handler.AppendLine(")");
        handler.AppendLine("values");
        handler.Append("(");
        foreach (var x in columns)
        {
            if (!x.IsMapped)
                continue;

            if (x.IsAutoIncrement)
                continue;

            handler.AppendLine();
            handler.Append("    ");
            if (this._useAmbientValue && x.AmbientValue is not null)
            {
                handler.Append(x.AmbientValue);
                handler.Append(',');
                continue;
            }
            handler.Append(prefix);
            handler.Append(x.MemberName);
            handler.Append(',');

            parameters ??= [];
            parameters.Add(x.MemberName, null);
        }
        handler.Advance(-1);
        handler.AppendLine();
        handler.Append(")");
    }
    #endregion
}
