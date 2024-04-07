using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QLimitive.UnitTests.SqlServer.Models;

namespace QLimitive.UnitTests.SqlServer.Cases;



[TestClass]
public sealed class DeleteTest
{
    private DbDialect Dialect { get; } = DbDialect.SqlServer;


    [TestMethod]
    public void All()
    {
        var actual = QueryBuilder.Delete<Person>(this.Dialect);
        var expect = "delete from [dbo].[T_People]";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().BeNull();
    }
}
