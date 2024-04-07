using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QLimitive.UnitTests.SqlServer.Models;

namespace QLimitive.UnitTests.SqlServer.Cases;



[TestClass]
public sealed class CountTest
{
    private DbDialect Dialect { get; } = DbDialect.SqlServer;


    [TestMethod]
    public void All()
    {
        var actual = QueryBuilder.Count<Person>(this.Dialect);
        var expect = "select count(*) as [Count] from [dbo].[T_People]";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().BeNull();
    }
}
