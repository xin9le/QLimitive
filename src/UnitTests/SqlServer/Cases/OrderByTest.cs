using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QLimitive.UnitTests.SqlServer.Models;

namespace QLimitive.UnitTests.SqlServer.Cases;



[TestClass]
public sealed class OrderByTest
{
    private DbDialect Dialect { get; } = DbDialect.SqlServer;


    [TestMethod]
    public void Ascending()
    {
        var actual = createQuery(this.Dialect);
        var expect =
@"order by
    [姓]";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().BeNull();

        static Query createQuery(DbDialect dialect)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                builder.OrderBy(static x => x.LastName);
                return builder.Build();
            }
        }
    }


    [TestMethod]
    public void Descending()
    {
        var actual = createQuery(this.Dialect);
        var expect =
@"order by
    [Age] desc";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().BeNull();

        static Query createQuery(DbDialect dialect)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                builder.OrderByDescending(static x => x.Age);
                return builder.Build();
            }
        }
    }
}
