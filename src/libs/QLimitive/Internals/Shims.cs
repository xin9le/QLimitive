#if !NET10_0_OR_GREATER
using System.Runtime.CompilerServices;
#endif

namespace QLimitive.Internals;



/// <summary>
/// Provides compatibility shims for APIs that are not available in all target .NET versions.
/// </summary>
internal static class Shims
{
#if !NET10_0_OR_GREATER
    extension(in DefaultInterpolatedStringHandler @this)
    {
        /// <summary>
        /// Clears the handler.
        /// </summary>
        public void Clear()
        {
            DefaultInterpolatedStringHandler_Clear(in @this);

            #region Local Functions
            [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "Clear")]
            static extern void DefaultInterpolatedStringHandler_Clear(in DefaultInterpolatedStringHandler handler);
            #endregion
        }
    }
#endif
}
