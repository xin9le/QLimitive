using System.Runtime.CompilerServices;
using QLimitive.Internals;

namespace QLimitive.Commands;



/// <summary>
/// Represents truncate command.
/// </summary>
/// <typeparam name="T"></typeparam>
internal readonly struct Truncate<T>(DbDialect dialect)
    : IQueryBuildable
{
    #region Fields
    private readonly DbDialect _dialect = dialect;
    #endregion


    #region IQueryBuildable
    /// <inheritdoc/>
    public void Build(ref DefaultInterpolatedStringHandler handler, ref BindParameterCollection? parameters)
    {
        handler.Append("truncate table ");
        handler.AppendTableName<T>(this._dialect);
    }
    #endregion
}
