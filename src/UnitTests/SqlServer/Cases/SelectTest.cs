﻿using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QLimitive.UnitTests.SqlServer.Models;

namespace QLimitive.UnitTests.SqlServer.Cases;



[TestClass]
public sealed class SelectTest
{
    private DbDialect Dialect { get; } = DbDialect.SqlServer;


    [TestMethod]
    public void AllColumns()
    {
        var actual = QueryBuilder.Select<Person>(this.Dialect);
        var expect =
@"select
    [Id] as [Id],
    [姓] as [LastName],
    [名] as [FirstName],
    [Age] as [Age],
    [Sex] as [Sex],
    [HasChildren] as [HasChildren],
    [CreatedAt] as [CreatedAt],
    [UpdatedAt] as [ModifiedAt]
from [dbo].[T_People]";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().BeNull();
    }


    [TestMethod]
    public void OneColumn()
    {
        var actual = QueryBuilder.Select<Person>(this.Dialect, static x => x.LastName);
        var expect =
@"select
    [姓] as [LastName]
from [dbo].[T_People]";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().BeNull();
    }


    [TestMethod]
    public void OneColumn_AnonymousType()
    {
        var actual = QueryBuilder.Select<Person>(this.Dialect, static x => new { x.LastName });
        var expect =
@"select
    [姓] as [LastName]
from [dbo].[T_People]";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().BeNull();
    }


    [TestMethod]
    public void TwoColumns()
    {
        var actual = QueryBuilder.Select<Person>(this.Dialect, static x => new { x.LastName, x.Age });
        var expect =
@"select
    [姓] as [LastName],
    [Age] as [Age]
from [dbo].[T_People]";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().BeNull();
    }


    [TestMethod]
    public void MultiColumns_IncludeNotMapped()
    {
        var actual = QueryBuilder.Select<Person>(this.Dialect, static x => new { x.LastName, x.FullName, x.Age });
        var expect =
@"select
    [姓] as [LastName],
    [Age] as [Age]
from [dbo].[T_People]";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().BeNull();
    }
}
