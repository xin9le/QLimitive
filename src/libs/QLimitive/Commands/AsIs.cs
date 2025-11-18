using Cysharp.Text;

namespace QLimitive.Commands;



/// <summary>
/// Provides delegate-style commands that can build query as primitive.<br/>
/// <b>This feature is provided for <i>as-is</i> use by those familiar with the internal implementation.</b>
/// </summary>
internal readonly struct AsIs : IQueryBuildable
{
    #region Properties
    /// <summary>
    /// Gets the delegating action for custom query building.
    /// </summary>
    private QueryBuildAction Action { get; }
    #endregion


    #region Constructors
    /// <summary>
    /// Creates instance.
    /// </summary>
    /// <param name="action"></param>
    public AsIs(QueryBuildAction action)
        => this.Action = action;
    #endregion


    #region IQueryBuildable implementations
    /// <inheritdoc/>
    public void Build(ref Utf16ValueStringBuilder builder, ref BindParameterCollection? parameters)
        => this.Action(ref builder, ref parameters);
    #endregion
}



/// <summary>
/// Provides delegate-style commands that can build query as primitive.<br/>
/// <b>This feature is provided for <i>as-is</i> use by those familiar with the internal implementation.</b>
/// </summary>
internal readonly struct AsIs<TState> : IQueryBuildable
{
    #region Properties
    /// <summary>
    /// Gets the delegating action for custom query building.
    /// </summary>
    private QueryBuildAction<TState> Action { get; }


    /// <summary>
    /// Gets the parameter for custom query building.
    /// </summary>
    private TState State { get; }
    #endregion


    #region Constructors
    /// <summary>
    /// Creates instance.
    /// </summary>
    /// <param name="action"></param>
    /// <param name="state">+</param>
    public AsIs(QueryBuildAction<TState> action, TState state)
    {
        this.Action = action;
        this.State = state;
    }
    #endregion


    #region IQueryBuildable implementations
    /// <inheritdoc/>
    public void Build(ref Utf16ValueStringBuilder builder, ref BindParameterCollection? parameters)
        => this.Action(ref builder, ref parameters, this.State);
    #endregion
}
