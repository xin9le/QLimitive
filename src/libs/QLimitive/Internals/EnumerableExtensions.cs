using System;
using System.Collections.Generic;
using System.Linq;

namespace QLimitive.Internals;



/// <summary>
/// Provides <see cref="IEnumerable{T}"/> extension methods
/// </summary>
internal static partial class EnumerableExtensions
{
    #region Materialize
    /// <summary>
    /// Gets collection count if <paramref name="source"/> is materialized, otherwise null.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static int? CountIfMaterialized<T>(this IEnumerable<T> source)
    {
        if (source == Enumerable.Empty<T>()) return 0;
        if (source == Array.Empty<T>()) return 0;
        if (source is ICollection<T> a) return a.Count;
        if (source is IReadOnlyCollection<T> b) return b.Count;

        return null;
    }
    #endregion
}
