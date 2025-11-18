using Cysharp.Text;
using QLimitive.Mappings;

namespace QLimitive.Internals;



/// <summary>
/// Provides the extension methods for <see cref="Utf16ValueStringBuilder"/>.
/// </summary>
internal static class StringBuilderExtensions
{
    /// <summary>
    /// Append full table name.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="builder"></param>
    /// <param name="dialect"></param>
    /// <remarks>- i.g.) [dbo].[TableName]</remarks>
    public static void AppendTableName<T>(ref this Utf16ValueStringBuilder builder, DbDialect dialect)
    {
        var table = TableMappingInfo.Get<T>();
        var schema = table.Schema ?? dialect.DefaultSchema;
        var bracket = dialect.KeywordBracket;
        if (string.IsNullOrWhiteSpace(schema))
        {
            builder.Append(bracket.Begin);
            builder.Append(table.Name);
            builder.Append(bracket.End);
        }
        else
        {
            builder.Append(bracket.Begin);
            builder.Append(schema);
            builder.Append(bracket.End);
            builder.Append('.');
            builder.Append(bracket.Begin);
            builder.Append(table.Name);
            builder.Append(bracket.End);
        }
    }
}
