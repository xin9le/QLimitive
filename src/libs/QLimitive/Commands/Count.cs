using System.Runtime.CompilerServices;
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
    public void Build(ref DefaultInterpolatedStringHandler handler, ref BindParameterCollection? parameters)
    {
        var bracket = this._dialect.KeywordBracket;
        handler.Append("select count(*) as ");
        handler.Append(bracket.Begin);
        handler.Append("Count");
        handler.Append(bracket.End);
        handler.Append(" from ");
        handler.AppendTableName<T>(this._dialect);
    }
    #endregion
}
