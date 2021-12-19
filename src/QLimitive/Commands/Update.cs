using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cysharp.Text;
using QLimitive.Internals;
using QLimitive.Mappings;

namespace QLimitive.Commands;



/// <summary>
/// Represents update command.
/// </summary>
/// <typeparam name="T"></typeparam>
internal readonly struct Update<T> : IQueryBuildable
{
    #region Properties
    /// <summary>
    /// Gets the database dialect.
    /// </summary>
    private DbDialect Dialect { get; }


    /// <summary>
    /// Gets the members mapped to the column.
    /// </summary>
    private Expression<Func<T, object?>>? Members { get; }


    /// <summary>
    /// Gets whether or not to use the default value.
    /// </summary>
    private bool UseDefaultValue { get; }
    #endregion


    #region Constructors
    /// <summary>
    /// Creates instance.
    /// </summary>
    public Update(DbDialect dialect, Expression<Func<T, object?>>? members, bool useDefaultValue)
    {
        this.Dialect = dialect;
        this.Members = members;
        this.UseDefaultValue = useDefaultValue;
    }
    #endregion


    #region IQueryBuildable implementations
    /// <inheritdoc/>
    public void Build(ref Utf16ValueStringBuilder builder, ref BindParameterCollection? parameters)
    {
        //--- Extract target columns
        HashSet<string>? targetMemberNames = null;
        if (this.Members is not null)
            targetMemberNames = ExpressionHelper.GetMemberNames(this.Members);

        //--- Build SQL
        var table = TableMappingInfo.Get<T>();
        var bracket = this.Dialect.KeywordBracket;
        var prefix = this.Dialect.BindParameterPrefix;

        builder.Append("update ");
        builder.AppendTableName<T>(this.Dialect);
        builder.AppendLine();
        builder.Append("set");
        foreach (var x in table.Columns)
        {
            if (!x.IsMapped)
                continue;

            if (targetMemberNames is null || targetMemberNames.Contains(x.MemberName))
            {
                builder.AppendLine();
                builder.Append("    ");
                builder.Append(bracket.Begin);
                builder.Append(x.ColumnName);
                builder.Append(bracket.End);
                builder.Append(" = ");
                if (this.UseDefaultValue && x.DefaultValue is not null)
                {
                    builder.Append(x.DefaultValue);
                    builder.Append(',');
                }
                else
                {
                    builder.Append(prefix);
                    builder.Append(x.MemberName);
                    builder.Append(',');

                    parameters ??= new BindParameterCollection();
                    parameters.Add(x.MemberName, null);
                }
            }
        }
        builder.Advance(-1);  // remove last colon
    }
    #endregion
}
