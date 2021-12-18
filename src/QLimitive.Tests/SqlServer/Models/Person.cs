using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLimitive.Tests.SqlServer.Models;



[Table("T_People", Schema = "dbo")]
public sealed class Person
{
#pragma warning disable CS8618
    [Key]
    public int Id { get; init; }


    [Column("名前")]
    public string Name { get; init; }


    public int Age { get; init; }


    public bool HasChildren { get; init; }


    [DefaultValue("SYSDATETIME()")]
    public DateTimeOffset CreatedAt { get; init; }


    [Column("UpdatedOn")]
    [DefaultValue("SYSDATETIME()")]
    public DateTimeOffset ModifiedAt { get; init; }
#pragma warning restore CS8618
}
