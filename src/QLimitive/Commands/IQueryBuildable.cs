using Cysharp.Text;

namespace QLimitive.Commands;



/// <summary>
/// Represents the SQL buildable.
/// </summary>
internal interface IQueryBuildable
{
    /// <summary>
    /// Build query.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="parameters"></param>
    void Build(in Utf16ValueStringBuilder builder, ref BindParameterCollection? parameters);
}
