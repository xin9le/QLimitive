using Cysharp.Text;
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
    public void Build(ref Utf16ValueStringBuilder builder, ref BindParameterCollection? parameters)
    {
        builder.Append("truncate table ");
        builder.AppendTableName<T>(this._dialect);
    }
    #endregion
}
