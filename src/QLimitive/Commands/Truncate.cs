using Cysharp.Text;
using QLimitive.Internals;

namespace QLimitive.Commands;



/// <summary>
/// Represents truncate command.
/// </summary>
/// <typeparam name="T"></typeparam>
internal readonly struct Truncate<T> : IQueryBuildable
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
    public Truncate(DbDialect dialect)
        => this.Dialect = dialect;
    #endregion


    #region IQueryBuildable implementations
    /// <inheritdoc/>
    public void Build(ref Utf16ValueStringBuilder builder, ref BindParameterCollection? parameters)
    {
        builder.Append("truncate table ");
        builder.AppendTableName<T>(this.Dialect);
    }
    #endregion
}
