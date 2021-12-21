using FluentAssertions;
using QLimitive.UnitTests.SqlServer.Models;
using Xunit;

namespace QLimitive.UnitTests.SqlServer.Cases;



public sealed class DeleteTest
{
    private DbDialect Dialect { get; } = DbDialect.SqlServer;


    [Fact]
    public void All()
    {
        var actual = QueryBuilder.Delete<Person>(this.Dialect);
        var expect = "delete from [dbo].[T_People]";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().BeNull();
    }
}
