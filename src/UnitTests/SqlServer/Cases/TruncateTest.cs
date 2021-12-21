using FluentAssertions;
using QLimitive.UnitTests.SqlServer.Models;
using Xunit;

namespace QLimitive.UnitTests.SqlServer.Cases;



public sealed class TruncateTest
{
    private DbDialect Dialect { get; } = DbDialect.SqlServer;


    [Fact]
    public void Create()
    {
        var actual = QueryBuilder.Truncate<Person>(this.Dialect);
        var expect = "truncate table [dbo].[T_People]";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().BeNull();
    }
}
