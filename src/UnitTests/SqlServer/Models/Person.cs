using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLimitive.UnitTests.SqlServer.Models;



[Table("T_People", Schema = "dbo")]
public sealed class Person
{
#pragma warning disable CS8618
    [Key]
    public int Id { get; init; }


    [Column("姓")]
    public string LastName { get; init; }


    [Column("名")]
    public string FirstName { get; init; }


    [NotMapped]
    public string FullName => $"{this.LastName} {this.FirstName}";


    public int Age { get; init; }


    public bool HasChildren { get; init; }


    [AmbientValue("SYSDATETIME()")]
    public DateTimeOffset CreatedAt { get; init; }


    [Column("UpdatedAt")]
    [AmbientValue("SYSDATETIME()")]
    public DateTimeOffset ModifiedAt { get; init; }
#pragma warning restore CS8618
}
