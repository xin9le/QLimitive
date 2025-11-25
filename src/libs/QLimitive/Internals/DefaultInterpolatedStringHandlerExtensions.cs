using System;
using System.Runtime.CompilerServices;
using QLimitive.Mappings;

namespace QLimitive.Internals;



/// <summary>
/// Provides the extension members for <see cref="DefaultInterpolatedStringHandler"/>.
/// </summary>
internal static class DefaultInterpolatedStringHandlerExtensions
{
    extension(ref DefaultInterpolatedStringHandler @this)
    {
        /// <summary>
        /// Writes the specified character span to the handler.
        /// </summary>
        /// <param name="value"></param>
        public void Append(string value)
            => @this.Append(value.AsSpan());


        /// <summary>
        /// Writes the specified character span to the handler.
        /// </summary>
        /// <param name="value"></param>
        public void Append(scoped ReadOnlySpan<char> value)
            => @this.AppendFormatted(value);


        /// <summary>
        /// Writes the specified value to the handler.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public void Append<T>(T value)
            => @this.AppendFormatted(value);


        /// <summary>
        /// Appends the default line terminator to the end of the current handler.
        /// </summary>
        public void AppendLine()
            => @this.AppendLiteral(Environment.NewLine);


        /// <summary>
        /// Writes the specified character span to the handler.
        /// </summary>
        /// <param name="value"></param>
        public void AppendLine(string value)
        {
            @this.Append(value.AsSpan());
            @this.AppendLine();
        }


        /// <summary>
        /// Writes the specified character span to the handler.
        /// </summary>
        /// <param name="value"></param>
        public void AppendLine(scoped ReadOnlySpan<char> value)
        {
            @this.Append(value);
            @this.AppendLine();
        }


        /// <summary>
        /// Writes the specified value to the handler.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public void AppendLine<T>(T value)
        {
            @this.Append(value);
            @this.AppendLine();
        }


        /// <summary>
        /// Append full table name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dialect"></param>
        /// <remarks>- i.g.) [dbo].[TableName]</remarks>
        public void AppendTableName<T>(DbDialect dialect)
        {
            var table = TableMappingInfo.Get<T>();
            var schema = table.Schema ?? dialect.DefaultSchema;
            var bracket = dialect.KeywordBracket;
            if (string.IsNullOrWhiteSpace(schema))
            {
                @this.Append(bracket.Begin);
                @this.Append(table.Name);
                @this.Append(bracket.End);
            }
            else
            {
                @this.Append(bracket.Begin);
                @this.Append(schema);
                @this.Append(bracket.End);
                @this.Append(".");
                @this.Append(bracket.Begin);
                @this.Append(table.Name);
                @this.Append(bracket.End);
            }
        }


        /// <summary>
        /// Notifies the handler that count data items were written to the internal buffer.
        /// </summary>
        /// <param name="count"></param>
        public void Advance(int count)
        {
            ref var pos = ref DefaultInterpolatedStringHandler_pos(in @this);
            pos += count;
        }


        /// <summary>
        /// Gets the length of the string written to the internal buffer.
        /// </summary>
        public int Length
            => DefaultInterpolatedStringHandler_pos(in @this);
    }


    #region UnsafeAccessors
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_pos")]
    private static extern ref int DefaultInterpolatedStringHandler_pos(in DefaultInterpolatedStringHandler handler);
    #endregion
}
