# QLimitive

**QLimitive** is an attribute-based primitive SQL generator that respects the Entity Framework Core and is called '*Primitive*'.


[![Releases](https://img.shields.io/github/release/xin9le/QLimitive.svg)](https://github.com/xin9le/QLimitive/releases)
[![Nuget packages](https://img.shields.io/nuget/v/QLimitive.svg)](https://www.nuget.org/packages/QLimitive/)
[![GitHub license](https://img.shields.io/github/license/xin9le/QLimitive)](https://github.com/xin9le/QLimitive/blob/master/LICENSE)
[![Unit Testing](https://github.com/xin9le/QLimitive/actions/workflows/test.yml/badge.svg)](https://github.com/xin9le/QLimitive/actions/workflows/test.yml)


## Support Platform

- .NET 8.0+



## Attribute-based O/R mapping information

**QLimitive** performs O/R mapping and generates SQL based on the attributes used in Entity Framework Core.


```cs
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleApp;

[Table("T_People", Schema = "dbo")]
public sealed class Person
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
}
```



## SQL generation

This library also provides automatic sql generation feature using above meta data. You can get very simple and typical SQL via `QueryBuilder` class. Of course it's completely type-safe.

```cs
// Query records with specified columns that matched specified condition
var sql = QueryBuilder.Select<Person>(DbDialect.SqlServer, x => new { x.LastName, x.Age }).Text;

/*
select
    [姓] as [LastName],
    [Age] as [Age]
from [dbo].[T_People]
*/
```


```cs
// If wants Where / OrderBy / ThenBy, allows you to write like following
using (var builder = new QueryBuilder<Person>(DbDialect.SqlServer))
{
    builder.Select(static x => new { x.Id, x.LastName });
    builder.Where(static x => x.LastName == "Suzuki");
    builder.OrderByDescending(static x => x.Age);
    builder.ThenBy(static x => x.ModifiedAt);
    var sql = builder.Build().Text;
}

/*
select
    [Id] as [Id],
    [姓] as [Name]
from [dbo].[T_People]
where
    [姓] = @p1
order by
    [Age] desc,
    [UpdatedAt]
*/
```

```cs
// Insert record to SQL Server
var sql = QueryBuilder.Insert<Person>(DbDialect.SqlServer, useAmbientValue: true).Text;

/*
insert into [dbo].[T_People]
(
    [姓],
    [名],
    [Age],
    [HasChildren],
    [CreatedAt],
    [UpdatedOn]
)
values
(
    @LastName,
    @FirstName,
    @Age,
    @HasChildren,
    SYSDATETIME(),
    SYSDATETIME()
)
*/
```

```cs
// Update records with specified columns that matched specified condition
using (var builder = new QueryBuilder<Person>(DbDialect.SqlServer))
{
    builder.Update(static x => new { x.LastName, x.Age, x.ModifiedAt }, useAmbientValue: true);
    builder.Where(static x => x.Age < 35 || x.LastName == "Suzuki");
    var sql = builder.Build().Text;
}

/*
update [dbo].[T_People]
set
    [姓] = @LastName,
    [Age] = @Age,
    [UpdatedAt] = SYSDATETIME()
where
    [Age] < @p3 or [姓] = @p4
*/
```


`QueryBuilder` class also provides some other overload functions and `Count` / `Delete` / `Truncate` methods, and so on.



## Installation

Getting started from downloading [NuGet](https://www.nuget.org/packages/QLimitive) package.

```
dotnet add package QLimitive
```



## License

This library is provided under [MIT License](http://opensource.org/licenses/MIT).



## Author

Takaaki Suzuki (a.k.a [@xin9le](https://about.xin9le.net)) is software developer in Japan who awarded Microsoft MVP for Developer Technologies (C#) since July 2012.
