using Cysharp.Text;
using QLimitive.Internals;
using QLimitive.Mappings;

namespace QLimitive.Commands;



/// <summary>
/// Represents insert command.
/// </summary>
/// <typeparam name="T"></typeparam>
internal readonly struct Insert<T> : IQueryBuildable
{
    #region Properties
    /// <summary>
    /// Gets the database dialect.
    /// </summary>
    private DbDialect Dialect { get; }


    /// <summary>
    /// Gets whether or not to use the ambient value.
    /// </summary>
    private bool UseAmbientValue { get; }
    #endregion


    #region Constructors
    /// <summary>
    /// Creates instance.
    /// </summary>
    public Insert(DbDialect dialect, bool useAmbientValue)
    {
        this.Dialect = dialect;
        this.UseAmbientValue = useAmbientValue;
    }
    #endregion


    #region IQueryBuildable implementations
    /// <inheritdoc/>
    public void Build(ref Utf16ValueStringBuilder builder, ref BindParameterCollection? parameters)
    {
        var table = TableMappingInfo.Get<T>();
        var bracket = this.Dialect.KeywordBracket;
        var prefix = this.Dialect.BindParameterPrefix;

        builder.Append("insert into ");
        builder.AppendTableName<T>(this.Dialect);
        builder.AppendLine();
        builder.Append("(");
        foreach (var x in table.Columns)
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
        foreach (var x in table.Columns)
        {
            if (!x.IsMapped)
                continue;

            if (x.IsAutoIncrement)
                continue;

            builder.AppendLine();
            builder.Append("    ");
            if (this.UseAmbientValue && x.AmbientValue is not null)
            {
                builder.Append(x.AmbientValue);
                builder.Append(',');
                continue;
            }
            builder.Append(prefix);
            builder.Append(x.MemberName);
            builder.Append(',');

            parameters ??= new BindParameterCollection();
            parameters.Add(x.MemberName, null);
        }
        builder.Advance(-1);
        builder.AppendLine();
        builder.Append(")");
    }
    #endregion
}
