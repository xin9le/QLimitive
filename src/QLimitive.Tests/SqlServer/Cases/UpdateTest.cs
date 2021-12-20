﻿using System.Collections.Generic;
using FluentAssertions;
using QLimitive.Tests.SqlServer.Models;
using Xunit;

namespace QLimitive.Tests.SqlServer.Cases;



public sealed class UpdateTest
{
    private DbDialect Dialect { get; } = DbDialect.SqlServer;


    [Fact]
    public void AllColumns()
    {
        var actual = QueryBuilder.Update<Person>(this.Dialect);
        var expect =
@"update [dbo].[T_People]
set
    [Id] = @Id,
    [姓] = @LastName,
    [名] = @FirstName,
    [Age] = @Age,
    [HasChildren] = @HasChildren,
    [CreatedAt] = @CreatedAt,
    [UpdatedOn] = @ModifiedAt";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain(new KeyValuePair<string, object?>[]
        {
            new("Id", null),
            new("LastName", null),
            new("FirstName", null),
            new("Age", null),
            new("HasChildren", null),
            new("CreatedAt", null),
            new("ModifiedAt", null),
        });
    }


    [Fact]
    public void AllColumns_UseDefaultValue()
    {
        var actual = QueryBuilder.Update<Person>(this.Dialect, useDefaultValue: true);
        var expect =
@"update [dbo].[T_People]
set
    [Id] = @Id,
    [姓] = @LastName,
    [名] = @FirstName,
    [Age] = @Age,
    [HasChildren] = @HasChildren,
    [CreatedAt] = SYSDATETIME(),
    [UpdatedOn] = SYSDATETIME()";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain(new KeyValuePair<string, object?>[]
        {
            new("Id", null),
            new("LastName", null),
            new("FirstName", null),
            new("Age", null),
            new("HasChildren", null),
        });
    }


    [Fact]
    public void OneColumn()
    {
        var actual = QueryBuilder.Update<Person>(this.Dialect, x => x.LastName);
        var expect =
@"update [dbo].[T_People]
set
    [姓] = @LastName";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain(new KeyValuePair<string, object?>("LastName", null));
    }


    [Fact]
    public void OneColumn_AnonymousType()
    {
        var actual = QueryBuilder.Update<Person>(this.Dialect, x => new { x.LastName });
        var expect =
@"update [dbo].[T_People]
set
    [姓] = @LastName";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain(new KeyValuePair<string, object?>("LastName", null));
    }


    [Fact]
    public void MultiColumns()
    {
        var actual = QueryBuilder.Update<Person>(this.Dialect, x => new { x.LastName, x.FullName, x.ModifiedAt });
        var expect =
@"update [dbo].[T_People]
set
    [姓] = @LastName,
    [UpdatedOn] = @ModifiedAt";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain(new KeyValuePair<string, object?>[]
        {
            new("LastName", null),
            new("ModifiedAt", null),
        });
    }


    [Fact]
    public void MultiColumns_UseDefaultValue()
    {
        var actual = QueryBuilder.Update<Person>(this.Dialect, x => new { x.LastName, x.FullName, x.ModifiedAt }, useDefaultValue: true);
        var expect =
@"update [dbo].[T_People]
set
    [姓] = @LastName,
    [UpdatedOn] = SYSDATETIME()";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain(new KeyValuePair<string, object?>("LastName", null));
    }
}