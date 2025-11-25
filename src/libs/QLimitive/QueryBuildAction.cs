using System.Runtime.CompilerServices;

namespace QLimitive;



/// <summary>
/// Represents the callback action that can build query as primitive.
/// </summary>
/// <param name="handler"></param>
/// <param name="bindParameters"></param>
public delegate void QueryBuildAction(ref DefaultInterpolatedStringHandler handler, ref BindParameterCollection? bindParameters);


/// <summary>
/// Represents the callback action that can build query as primitive.
/// </summary>
/// <param name="handler"></param>
/// <param name="bindParameters"></param>
/// <param name="state"></param>
public delegate void QueryBuildAction<TState>(ref DefaultInterpolatedStringHandler handler, ref BindParameterCollection? bindParameters, TState state);
