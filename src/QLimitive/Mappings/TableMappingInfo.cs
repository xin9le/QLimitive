using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using QLimitive.Internals;

namespace QLimitive.Mappings;



/// <summary>
/// Provides table mapping information.
/// </summary>
internal sealed class TableMappingInfo
{
    #region Properties
    /// <summary>
    /// Gets the type that is mapped to the table.
    /// </summary>
    public Type Type { get; private init; }


    /// <summary>
    /// Gets the schema name.
    /// </summary>
    public string? Schema { get; private init; }


    /// <summary>
    /// Gets the table name.
    /// </summary>
    public string Name { get; private init; }


    /// <summary>
    /// Gets the column mapping information.
    /// </summary>
    public ReadOnlyArray<ColumnMappingInfo> Columns { get; private init; }


    /// <summary>
    /// Gets the column mapping information by member name.
    /// </summary>
    public FrozenStringKeyDictionary<ColumnMappingInfo> ColumnByMemberName { get; private init; }
    #endregion


    #region Constructors
#pragma warning disable CS8618
    /// <summary>
    /// Creates instance.
    /// </summary>
    private TableMappingInfo()
    { }
#pragma warning restore CS8618
    #endregion


    #region Get
    /// <summary>
    /// Gets the table mapping information corresponding to the specified type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TableMappingInfo Get<T>()
        => Cache<T>.Instance;
    #endregion


    #region Internal Cache
    /// <summary>
    /// Provides <see cref="TableMappingInfo"/> cache.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private static class Cache<T>
    {
        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static TableMappingInfo Instance { get; }


        /// <summary>
        /// Static constructors
        /// </summary>
        static Cache()
        {
            var type = typeof(T);
            var tableAttr = type.GetCustomAttributes<TableAttribute>(true).FirstOrDefault();
            var columns = getColumns().ToReadOnlyArray();
            Instance = new()
            {
                Type = type,
                Schema = tableAttr?.Schema,
                Name = tableAttr?.Name ?? type.Name,
                Columns = columns,
                ColumnByMemberName = columns.ToFrozenStringKeyDictionary(x => x.MemberName),
            };

            #region Local Functions
            static IEnumerable<ColumnMappingInfo> getColumns()
            {
                var type = typeof(T);
                var flags = BindingFlags.Instance | BindingFlags.Public;
                var nullabilityContext = new NullabilityInfoContext();

                //--- Check if exists composite primary key
                var properties = type.GetProperties(flags);
                var fields = type.GetFields(flags);
                var primaryKeyCount
                    = properties.Cast<MemberInfo>()
                    .Concat(fields.Cast<MemberInfo>())
                    .Count(static x => Attribute.IsDefined(x, typeof(KeyAttribute)));
                var existsCompositePrimaryKey = primaryKeyCount >= 2;

                //--- Properties
                foreach (var property in properties)
                {
                    var nullability = nullabilityContext.Create(property);
                    yield return new ColumnMappingInfo(property, nullability, existsCompositePrimaryKey);
                }

                //--- Fields
                foreach (var field in fields)
                {
                    var nullability = nullabilityContext.Create(field);
                    yield return new ColumnMappingInfo(field, nullability, existsCompositePrimaryKey);
                }
            }
            #endregion
        }
    }
    #endregion
}
