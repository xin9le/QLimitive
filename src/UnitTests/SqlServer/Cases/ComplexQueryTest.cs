using System.Collections.Generic;
using FluentAssertions;
using QLimitive.UnitTests.SqlServer.Models;
using Xunit;

namespace QLimitive.UnitTests.SqlServer.Cases;



public sealed class ComplexQueryTest
{
    private DbDialect Dialect { get; } = DbDialect.SqlServer;


    [Fact]
    public void CountWhere1()
    {
        var actual = createQuery(this.Dialect);
        var expect =
@"select count(*) as [Count] from [dbo].[T_People]
where
    [Id] = @p1";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain("p1", 1);

        static Query createQuery(DbDialect dialect)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                builder.Count();
                builder.Where(static x => x.Id == 1);
                return builder.Build();
            }
        }
    }


    [Fact]
    public void CountWhere2()
    {
        var actual = createQuery(this.Dialect);
        var expect =
@"select count(*) as [Count] from [dbo].[T_People]
where
    [Id] = @p1 and [姓] <> @p2";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain("p1", 1);
        actual.Parameters.Should().Contain("p2", "xin9le");

        static Query createQuery(DbDialect dialect)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                builder.Count();
                builder.Where(static x => x.Id == 1 && x.LastName != "xin9le");
                return builder.Build();
            }
        }
    }


    [Fact]
    public void CountWhere3()
    {
        var actual = createQuery(this.Dialect);
        var expect =
@"select count(*) as [Count] from [dbo].[T_People]
where
    [Id] = @p1 or [姓] <> @p2";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain("p1", 1);
        actual.Parameters.Should().Contain("p2", "xin9le");

        static Query createQuery(DbDialect dialect)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                builder.Count();
                builder.Where(static x => x.Id == 1 || x.LastName != "xin9le");
                return builder.Build();
            }
        }
    }


    [Fact]
    public void SelectWhere()
    {
        var actual = createQuery(this.Dialect);
        var expect =
@"select
    [Id] as [Id],
    [姓] as [LastName],
    [名] as [FirstName],
    [Age] as [Age],
    [HasChildren] as [HasChildren],
    [CreatedAt] as [CreatedAt],
    [UpdatedAt] as [ModifiedAt]
from [dbo].[T_People]
where
    [Id] = @p1 or [姓] <> @p2";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain("p1", 1);
        actual.Parameters.Should().Contain("p2", "xin9le");

        static Query createQuery(DbDialect dialect)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                builder.Select();
                builder.Where(static x => x.Id == 1 || x.LastName != "xin9le");
                return builder.Build();
            }
        }
    }


    [Fact]
    public void SelectWhereOrderByThenBy()
    {
        var actual = createQuery(this.Dialect);
        var expect =
@"select
    [Id] as [Id],
    [姓] as [LastName],
    [名] as [FirstName],
    [Age] as [Age],
    [HasChildren] as [HasChildren],
    [CreatedAt] as [CreatedAt],
    [UpdatedAt] as [ModifiedAt]
from [dbo].[T_People]
where
    [Id] = @p1 or ([姓] <> @p2 and [Age] > @p3)
order by
    [Id],
    [Age] desc";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain("p1", 1);
        actual.Parameters.Should().Contain("p2", "xin9le");
        actual.Parameters.Should().Contain("p3", 20);

        static Query createQuery(DbDialect dialect)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                builder.Select();
                builder.Where(static x => x.Id == 1 || x.LastName != "xin9le" && x.Age > 20);
                builder.OrderBy(static x => x.Id);
                builder.ThenByDescending(static x => x.Age);
                return builder.Build();
            }
        }
    }


    [Fact]
    public void UpdateWhere()
    {
        var actual = createQuery(this.Dialect);
        var expect =
@"update [dbo].[T_People]
set
    [Age] = @Age,
    [UpdatedAt] = SYSDATETIME()
where
    [Id] = @p2 or [姓] <> @p3";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain(new KeyValuePair<string, object?>[]
        {
            new("Age", null),
            new("p2", 1),
            new("p3", "xin9le"),
        });

        static Query createQuery(DbDialect dialect)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                builder.Update(static x => new { x.Age, x.ModifiedAt }, useDefaultValue: true);
                builder.Where(static x => x.Id == 1 || x.LastName != "xin9le");
                return builder.Build();
            }
        }
    }


    [Fact]
    public void DeleteWhere()
    {
        var actual = createQuery(this.Dialect);
        var expect =
@"delete from [dbo].[T_People]
where
    [Id] = @p1 or [姓] <> @p2";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain("p1", 1);
        actual.Parameters.Should().Contain("p2", "xin9le");

        static Query createQuery(DbDialect dialect)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                builder.Delete();
                builder.Where(static x => x.Id == 1 || x.LastName != "xin9le");
                return builder.Build();
            }
        }
    }
}
