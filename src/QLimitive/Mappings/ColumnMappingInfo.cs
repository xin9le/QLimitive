using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace QLimitive.Mappings;



/// <summary>
/// Provides column mapping information.
/// </summary>
internal sealed class ColumnMappingInfo
{
    #region Properties
    /// <summary>
    /// Gets the name of member.
    /// </summary>
    public string MemberName { get; }


    /// <summary>
    /// Gets the type of member.
    /// </summary>
    public Type MemberType { get; }


    /// <summary>
    /// Gets the name of column.
    /// </summary>
    public string ColumnName { get; }


    /// <summary>
    /// Gets the type name of column.
    /// </summary>
    public string? ColumnTypeName { get; }


    /// <summary>
    /// Gets the order of column.
    /// </summary>
    public int ColumnOrder { get; }


    /// <summary>
    /// Gets the default value.
    /// </summary>
    public object? DefaultValue { get; }


    /// <summary>
    /// Gets whether primary key.
    /// </summary>
    public bool IsPrimaryKey { get; }


    /// <summary>
    /// Gets whether allowing null.
    /// </summary>
    public bool IsNullable { get; }


    /// <summary>
    /// Gets whether column mapping.
    /// </summary>
    public bool IsMapped { get; }


    /// <summary>
    /// Gets whether auto increment numbering.
    /// </summary>
    public bool IsAutoIncrement { get; }
    #endregion


    #region Constructors
    /// <summary>
    /// Creates instance.
    /// </summary>
    public ColumnMappingInfo(PropertyInfo property, NullabilityInfo nullability, bool existsCompositePrimaryKey)
        : this(property, property.PropertyType, nullability, existsCompositePrimaryKey)
    { }


    /// <summary>
    /// Creates instance.
    /// </summary>
    public ColumnMappingInfo(FieldInfo field, NullabilityInfo nullability, bool existsCompositePrimaryKey)
        : this(field, field.FieldType, nullability, existsCompositePrimaryKey)
    { }


    /// <summary>
    /// Creates instance.
    /// </summary>
    private ColumnMappingInfo(MemberInfo member, Type memberType, NullabilityInfo nullability, bool existsCompositePrimaryKey)
    {
        var columnAttr = member.GetCustomAttributes<ColumnAttribute>(true).FirstOrDefault();
        var defaultAttr = member.GetCustomAttributes<DefaultValueAttribute>(true).FirstOrDefault();
        var dbGeneratedAttr = member.GetCustomAttributes<DatabaseGeneratedAttribute>(true).FirstOrDefault();

        this.MemberName = member.Name;
        this.MemberType = memberType;
        this.ColumnName = columnAttr?.Name ?? member.Name;
        this.ColumnTypeName = columnAttr?.TypeName;
        this.ColumnOrder = columnAttr?.Order ?? -1;
        this.DefaultValue = defaultAttr?.Value;
        this.IsPrimaryKey = Attribute.IsDefined(member, typeof(KeyAttribute));
        this.IsNullable = nullability.ReadState == NullabilityState.Nullable;
        this.IsMapped = !Attribute.IsDefined(member, typeof(NotMappedAttribute));
        this.IsAutoIncrement = isAutoIncrement(dbGeneratedAttr, memberType, this.IsPrimaryKey, existsCompositePrimaryKey);

        #region Local Functions
        static bool isAutoIncrement(DatabaseGeneratedAttribute? attr, Type memberType, bool isPrimaryKey, bool existsCompositePrimaryKey)
        {
            //--- Explicit determination by attributes
            if (attr is not null)
                return attr.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity;

            //--- Automatic determination
            if (isPrimaryKey && !existsCompositePrimaryKey)
                return isNumberType(memberType);

            return false;
        }


        static bool isNumberType(Type type)
            => Type.GetTypeCode(type)
            is TypeCode.Byte
            or TypeCode.SByte
            or TypeCode.UInt16
            or TypeCode.UInt32
            or TypeCode.UInt64
            or TypeCode.Int16
            or TypeCode.Int32
            or TypeCode.Int64;
        #endregion
    }
    #endregion
}
