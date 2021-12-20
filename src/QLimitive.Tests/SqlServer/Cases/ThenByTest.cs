using FluentAssertions;
using QLimitive.Tests.SqlServer.Models;
using Xunit;

namespace QLimitive.Tests.SqlServer.Cases;



public sealed class ThenByTest
{
    private DbDialect Dialect { get; } = DbDialect.SqlServer;


    [Fact]
    public void Ascending()
    {
        var actual = createQuery(this.Dialect);
        var expect =
@"order by
    [姓],
    [Age]";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().BeNull();

        static Query createQuery(DbDialect dialect)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                builder.OrderBy(static x => x.LastName);
                builder.ThenBy(static x => x.Age);
                return builder.Build();
            }
        }
    }


    [Fact]
    public void Descending()
    {
        var actual = createQuery(this.Dialect);
        var expect =
@"order by
    [Age] desc,
    [名] desc";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().BeNull();

        static Query createQuery(DbDialect dialect)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                builder.OrderByDescending(static x => x.Age);
                builder.ThenByDescending(static x => x.FirstName);
                return builder.Build();
            }
        }
    }


    [Fact]
    public void Complex()
    {
        var actual = createQuery(this.Dialect);
        var expect =
@"order by
    [姓],
    [Age] desc,
    [名],
    [CreatedAt] desc";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().BeNull();

        static Query createQuery(DbDialect dialect)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                builder.OrderBy(static x => x.LastName);
                builder.ThenByDescending(static x => x.Age);
                builder.ThenBy(static x => x.FirstName);
                builder.ThenByDescending(static x => x.CreatedAt);
                return builder.Build();
            }
        }
    }
}
