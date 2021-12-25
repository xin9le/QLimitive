using System.Collections.Generic;
using FluentAssertions;
using QLimitive.UnitTests.SqlServer.Models;
using Xunit;

namespace QLimitive.UnitTests.SqlServer.Cases;



public sealed class InsertTest
{
    private DbDialect Dialect { get; } = DbDialect.SqlServer;


    [Fact]
    public void ReferencePropertyValue()
    {
        var actual = QueryBuilder.Insert<Person>(this.Dialect, useAmbientValue: false);
        var expect =
@"insert into [dbo].[T_People]
(
    [姓],
    [名],
    [Age],
    [Sex],
    [HasChildren],
    [CreatedAt],
    [UpdatedAt]
)
values
(
    @LastName,
    @FirstName,
    @Age,
    @Sex,
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
            new("Sex", null),
            new("HasChildren", null),
            new("CreatedAt", null),
            new("ModifiedAt", null),
        });
    }


    [Fact]
    public void ReferenceAmbientValue()
    {
        var actual = QueryBuilder.Insert<Person>(this.Dialect, useAmbientValue: true);
        var expect =
@"insert into [dbo].[T_People]
(
    [姓],
    [名],
    [Age],
    [Sex],
    [HasChildren],
    [CreatedAt],
    [UpdatedAt]
)
values
(
    @LastName,
    @FirstName,
    @Age,
    @Sex,
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
            new("Sex", null),
            new("HasChildren", null),
        });
    }
}
