using FluentAssertions;
using QLimitive.Tests.SqlServer.Models;
using Xunit;

namespace QLimitive.Tests.SqlServer.Cases;



public sealed class DeleteTest
{
    private DbDialect Dialect { get; } = DbDialect.SqlServer;


    [Fact]
    public void Create()
    {
        var actual = QueryBuilder.Delete<Person>(this.Dialect);
        var expect = "delete from [dbo].[T_People]";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().BeNull();
    }
}
