using System.Runtime.CompilerServices;

namespace QLimitive.Internals;



/// <summary>
/// Provides compatibility shims for APIs that are not available in all target .NET versions.
/// </summary>
internal static class Shims
{
#if !NET10_0_OR_GREATER
    extension(ref DefaultInterpolatedStringHandler @this)
    {
        public void Clear()
        {
            DefaultInterpolatedStringHandler_Clear(ref @this);

            #region Local Functions
            [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "Clear")]
            static extern void DefaultInterpolatedStringHandler_Clear(ref DefaultInterpolatedStringHandler handler);
            #endregion
        }
    }
#endif


    extension(Unsafe)
    {
        public static unsafe void* AsPointer(ref DefaultInterpolatedStringHandler value)
        {
#if NET10_0_OR_GREATER
            return Unsafe.AsPointer(ref value);
#else
            var fp = (delegate*<ref DefaultInterpolatedStringHandler, void*>)(delegate*<ref nint, void*>)&Unsafe.AsPointer<nint>;
            return fp(ref value);
#endif
        }


        public static unsafe ref DefaultInterpolatedStringHandler AsRef(void* source)
        {
#if NET10_0_OR_GREATER
            return ref Unsafe.AsRef<DefaultInterpolatedStringHandler>(source);
#else
            var fp = (delegate*<void*, ref DefaultInterpolatedStringHandler>)(delegate*<void*, ref nint>)&Unsafe.AsRef<nint>;
            return ref fp(source);
#endif
        }
    }
}
