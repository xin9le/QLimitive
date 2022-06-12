using Cysharp.Text;

namespace QLimitive;



/// <summary>
/// Represents the callback action that can build query as primitive.
/// </summary>
/// <param name="builder"></param>
/// <param name="bindParameters"></param>
public delegate void QueryBuildAction(ref Utf16ValueStringBuilder builder, ref BindParameterCollection? bindParameters);


/// <summary>
/// Represents the callback action that can build query as primitive.
/// </summary>
/// <param name="builder"></param>
/// <param name="bindParameters"></param>
/// <param name="state"></param>
public delegate void QueryBuildAction<TState>(ref Utf16ValueStringBuilder builder, ref BindParameterCollection? bindParameters, TState state);
