using Cysharp.Text;
using QLimitive.Internals;

namespace QLimitive.Commands;



/// <summary>
/// Represents count command.
/// </summary>
/// <typeparam name="T"></typeparam>
internal readonly struct Count<T>(DbDialect dialect)
    : IQueryBuildable
{
    #region Fields
    private readonly DbDialect _dialect = dialect;
    #endregion


    #region IQueryBuildable
    /// <inheritdoc/>
    public void Build(ref Utf16ValueStringBuilder builder, ref BindParameterCollection? parameters)
    {
        var bracket = this._dialect.KeywordBracket;
        builder.Append("select count(*) as ");
        builder.Append(bracket.Begin);
        builder.Append("Count");
        builder.Append(bracket.End);
        builder.Append(" from ");
        builder.AppendTableName<T>(this._dialect);
    }
    #endregion
}
