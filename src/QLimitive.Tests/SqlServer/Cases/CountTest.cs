﻿using Cysharp.Text;
using FluentAssertions;
using QLimitive.Tests.SqlServer.Models;
using Xunit;

namespace QLimitive.Tests.SqlServer.Cases;



public sealed class CountTest
{
    private DbDialect Dialect { get; } = DbDialect.SqlServer;


    [Fact]
    public void All()
    {
        var actual = QueryBuilder.Count<Person>(this.Dialect);
        var expect = "select count(*) as [Count] from [dbo].[T_People]";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().BeNull();
    }
}
