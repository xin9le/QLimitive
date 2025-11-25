using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace QLimitive.Mappings;



/// <summary>
/// Provides table mapping information.
/// </summary>
public sealed class TableMappingInfo
{
    #region Properties
    /// <summary>
    /// Gets the type that is mapped to the table.
    /// </summary>
    public Type Type { get; }


    /// <summary>
    /// Gets the schema name.
    /// </summary>
    public string? Schema { get; }


    /// <summary>
    /// Gets the table name.
    /// </summary>
    public string Name { get; }


    /// <summary>
    /// Gets the column mapping information.
    /// </summary>
    public ImmutableArray<ColumnMappingInfo> Columns { get; }


    /// <summary>
    /// Gets the column mapping information by member name.
    /// </summary>
    /// <remarks>provides fast access.</remarks>
    public FrozenDictionary<string, ColumnMappingInfo> ColumnByMemberName { get; }
    #endregion


    #region Constructors
    /// <summary>
    /// Creates instance.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="table"></param>
    /// <param name="columns"></param>
    private TableMappingInfo(Type type, TableAttribute? table, ImmutableArray<ColumnMappingInfo> columns)
    {
        this.Type = type;
        this.Schema = table?.Schema;
        this.Name = table?.Name ?? type.Name;
        this.Columns = columns;
        this.ColumnByMemberName = columns.ToFrozenDictionary(static x => x.MemberName);
    }
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
            var table = type.GetCustomAttributes<TableAttribute>(true).FirstOrDefault();
            var columns = getColumns().ToImmutableArray();
            Instance = new(type, table, columns);

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
