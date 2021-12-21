using System.Collections.Generic;
using FluentAssertions;
using QLimitive.Tests.SqlServer.Models;
using Xunit;

namespace QLimitive.Tests.SqlServer.Cases;



public sealed class InsertTest
{
    private DbDialect Dialect { get; } = DbDialect.SqlServer;


    [Fact]
    public void ReferencePropertyValue()
    {
        var actual = QueryBuilder.Insert<Person>(this.Dialect, useDefaultValue: false);
        var expect =
@"insert into [dbo].[T_People]
(
    [姓],
    [名],
    [Age],
    [HasChildren],
    [CreatedAt],
    [UpdatedAt]
)
values
(
    @LastName,
    @FirstName,
    @Age,
    @HasChildren,
    @CreatedAt,
    @ModifiedAt
)";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain(new KeyValuePair<string, object?>[]
        {
            new("LastName", null),
            new("FirstName", null),
            new("Age", null),
            new("HasChildren", null),
            new("CreatedAt", null),
            new("ModifiedAt", null),
        });
    }


    [Fact]
    public void ReferenceDefaultValue()
    {
        var actual = QueryBuilder.Insert<Person>(this.Dialect, useDefaultValue: true);
        var expect =
@"insert into [dbo].[T_People]
(
    [姓],
    [名],
    [Age],
    [HasChildren],
    [CreatedAt],
    [UpdatedAt]
)
values
(
    @LastName,
    @FirstName,
    @Age,
    @HasChildren,
    SYSDATETIME(),
    SYSDATETIME()
)";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain(new KeyValuePair<string, object?>[]
        {
            new("LastName", null),
            new("FirstName", null),
            new("Age", null),
            new("HasChildren", null),
        });
    }
}
