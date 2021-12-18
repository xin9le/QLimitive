using Cysharp.Text;
using QLimitive.Internals;

namespace QLimitive.Commands;



/// <summary>
/// Represents count command.
/// </summary>
/// <typeparam name="T"></typeparam>
internal readonly struct Count<T> : IQueryBuildable
{
    #region Properties
    /// <summary>
    /// Gets the database dialect.
    /// </summary>
    private DbDialect Dialect { get; }
    #endregion


    #region Constructors
    /// <summary>
    /// Creates instance.
    /// </summary>
    public Count(DbDialect dialect)
        => this.Dialect = dialect;
    #endregion


    #region IQueryBuildable implementations
    /// <inheritdoc/>
    public void Build(in Utf16ValueStringBuilder builder, ref BindParameterCollection? parameters)
    {
        var bracket = this.Dialect.KeywordBracket;
        builder.Append("select count(*) as ");
        builder.Append(bracket.Begin);
        builder.Append("Count");
        builder.Append(bracket.End);
        builder.Append(" from ");
        builder.AppendTableName<T>(this.Dialect);
    }
    #endregion
}
