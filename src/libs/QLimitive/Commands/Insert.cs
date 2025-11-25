using Cysharp.Text;
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
    public void Build(ref Utf16ValueStringBuilder builder, ref BindParameterCollection? parameters)
    {
        var table = TableMappingInfo.Get<T>();
        var columns = table.Columns.Span;
        var bracket = this._dialect.KeywordBracket;
        var prefix = this._dialect.BindParameterPrefix;

        builder.Append("insert into ");
        builder.AppendTableName<T>(this._dialect);
        builder.AppendLine();
        builder.Append("(");
        foreach (var x in columns)
        {
            if (!x.IsMapped)
                continue;

            if (x.IsAutoIncrement)
                continue;

            builder.AppendLine();
            builder.Append("    ");
            builder.Append(bracket.Begin);
            builder.Append(x.ColumnName);
            builder.Append(bracket.End);
            builder.Append(',');
        }
        builder.Advance(-1);
        builder.AppendLine();
        builder.AppendLine(")");
        builder.AppendLine("values");
        builder.Append("(");
        foreach (var x in columns)
        {
            if (!x.IsMapped)
                continue;

            if (x.IsAutoIncrement)
                continue;

            builder.AppendLine();
            builder.Append("    ");
            if (this._useAmbientValue && x.AmbientValue is not null)
            {
                builder.Append(x.AmbientValue);
                builder.Append(',');
                continue;
            }
            builder.Append(prefix);
            builder.Append(x.MemberName);
            builder.Append(',');

            parameters ??= [];
            parameters.Add(x.MemberName, null);
        }
        builder.Advance(-1);
        builder.AppendLine();
        builder.Append(")");
    }
    #endregion
}
