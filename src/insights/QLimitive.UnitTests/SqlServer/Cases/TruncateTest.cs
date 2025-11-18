using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QLimitive.UnitTests.SqlServer.Models;

namespace QLimitive.UnitTests.SqlServer.Cases;



[TestClass]
public sealed class TruncateTest
{
    private DbDialect Dialect { get; } = DbDialect.SqlServer;


    [TestMethod]
    public void Create()
    {
        var actual = QueryBuilder.Truncate<Person>(this.Dialect);
        var expect = "truncate table [dbo].[T_People]";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().BeNull();
    }
}
