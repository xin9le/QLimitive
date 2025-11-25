using System.Runtime.CompilerServices;

namespace QLimitive.Commands;



/// <summary>
/// Represents the SQL buildable.
/// </summary>
internal interface IQueryBuildable
{
    /// <summary>
    /// Build query.
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="parameters"></param>
    void Build(ref DefaultInterpolatedStringHandler handler, ref BindParameterCollection? parameters);
}
