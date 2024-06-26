﻿using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QLimitive.UnitTests.SqlServer.Models;

namespace QLimitive.UnitTests.SqlServer.Cases;



[TestClass]
public sealed class UpdateTest
{
    private DbDialect Dialect { get; } = DbDialect.SqlServer;


    [TestMethod]
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
    [Sex] = @Sex,
    [HasChildren] = @HasChildren,
    [CreatedAt] = @CreatedAt,
    [UpdatedAt] = @ModifiedAt";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain(
        [
            new("Id", null),
            new("LastName", null),
            new("FirstName", null),
            new("Age", null),
            new("Sex", null),
            new("HasChildren", null),
            new("CreatedAt", null),
            new("ModifiedAt", null),
        ]);
    }


    [TestMethod]
    public void AllColumns_UseAmbientValue()
    {
        var actual = QueryBuilder.Update<Person>(this.Dialect, useAmbientValue: true);
        var expect =
@"update [dbo].[T_People]
set
    [Id] = @Id,
    [姓] = @LastName,
    [名] = @FirstName,
    [Age] = @Age,
    [Sex] = @Sex,
    [HasChildren] = @HasChildren,
    [CreatedAt] = SYSDATETIME(),
    [UpdatedAt] = SYSDATETIME()";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain(
        [
            new("Id", null),
            new("LastName", null),
            new("FirstName", null),
            new("Age", null),
            new("Sex", null),
            new("HasChildren", null),
        ]);
    }


    [TestMethod]
    public void OneColumn()
    {
        var actual = QueryBuilder.Update<Person>(this.Dialect, static x => x.LastName);
        var expect =
@"update [dbo].[T_People]
set
    [姓] = @LastName";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain(new KeyValuePair<string, object?>("LastName", null));
    }


    [TestMethod]
    public void OneColumn_AnonymousType()
    {
        var actual = QueryBuilder.Update<Person>(this.Dialect, static x => new { x.LastName });
        var expect =
@"update [dbo].[T_People]
set
    [姓] = @LastName";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain(new KeyValuePair<string, object?>("LastName", null));
    }


    [TestMethod]
    public void MultiColumns()
    {
        var actual = QueryBuilder.Update<Person>(this.Dialect, static x => new { x.LastName, x.FullName, x.ModifiedAt });
        var expect =
@"update [dbo].[T_People]
set
    [姓] = @LastName,
    [UpdatedAt] = @ModifiedAt";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain(
        [
            new("LastName", null),
            new("ModifiedAt", null),
        ]);
    }


    [TestMethod]
    public void MultiColumns_UseAmbientValue()
    {
        var actual = QueryBuilder.Update<Person>(this.Dialect, static x => new { x.LastName, x.FullName, x.ModifiedAt }, useAmbientValue: true);
        var expect =
@"update [dbo].[T_People]
set
    [姓] = @LastName,
    [UpdatedAt] = SYSDATETIME()";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain(new KeyValuePair<string, object?>("LastName", null));
    }
}
