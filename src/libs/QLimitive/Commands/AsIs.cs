using System.Runtime.CompilerServices;

namespace QLimitive.Commands;



/// <summary>
/// Provides delegate-style commands that can build query as primitive.<br/>
/// <b>This feature is provided for <i>as-is</i> use by those familiar with the internal implementation.</b>
/// </summary>
internal readonly struct AsIs(QueryBuildAction action)
    : IQueryBuildable
{
    #region Fields
    private readonly QueryBuildAction _action = action;
    #endregion


    #region IQueryBuildable
    /// <inheritdoc/>
    public void Build(ref DefaultInterpolatedStringHandler handler, ref BindParameterCollection? parameters)
        => this._action(ref handler, ref parameters);
    #endregion
}



/// <summary>
/// Provides delegate-style commands that can build query as primitive.<br/>
/// <b>This feature is provided for <i>as-is</i> use by those familiar with the internal implementation.</b>
/// </summary>
internal readonly struct AsIs<TState>(QueryBuildAction<TState> action, TState state) : IQueryBuildable
{
    #region Fields
    private readonly QueryBuildAction<TState> _action = action;
    private readonly TState _state = state;
    #endregion


    #region IQueryBuildable
    /// <inheritdoc/>
    public void Build(ref DefaultInterpolatedStringHandler handler, ref BindParameterCollection? parameters)
        => this._action(ref handler, ref parameters, this._state);
    #endregion
}
