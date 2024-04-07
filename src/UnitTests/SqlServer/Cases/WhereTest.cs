using System;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QLimitive.UnitTests.SqlServer.Models;

namespace QLimitive.UnitTests.SqlServer.Cases;



[TestClass]
public sealed class WhereTest
{
    private DbDialect Dialect { get; } = DbDialect.SqlServer;


    [TestMethod]
    public void Equal()
    {
        var actual = createQuery(this.Dialect);
        var expect =
@"where
    [Id] = @p1";

        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain("p1", 1);

        static Query createQuery(DbDialect dialect)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                builder.Where(static x => x.Id == 1);
                return builder.Build();
            }
        }
    }


    [TestMethod]
    public void NotEqual()
    {
        var actual = createQuery(this.Dialect);
        var expect =
@"where
    [Id] <> @p1";

        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain("p1", 1);

        static Query createQuery(DbDialect dialect)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                builder.Where(static x => x.Id != 1);
                return builder.Build();
            }
        }
    }


    [TestMethod]
    public void GreaterThan()
    {
        var actual = createQuery(this.Dialect);
        var expect =
@"where
    [Id] > @p1";

        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain("p1", 1);

        static Query createQuery(DbDialect dialect)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                builder.Where(static x => x.Id > 1);
                return builder.Build();
            }
        }
    }


    [TestMethod]
    public void LessThan()
    {
        var actual = createQuery(this.Dialect);
        var expect =
@"where
    [Id] < @p1";

        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain("p1", 1);
  
        static Query createQuery(DbDialect dialect)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                builder.Where(static x => x.Id < 1);
                return builder.Build();
            }
        }
    }


    [TestMethod]
    public void GreaterThanOrEqual()
    {
        var actual = createQuery(this.Dialect);
        var expect =
@"where
    [Id] >= @p1";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain("p1", 1);

        static Query createQuery(DbDialect dialect)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                builder.Where(static x => x.Id >= 1);
                return builder.Build();
            }
        }
    }


    [TestMethod]
    public void LessThanOrEqual()
    {
        var actual = createQuery(this.Dialect);
        var expect =
@"where
    [Id] <= @p1";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain("p1", 1);

        static Query createQuery(DbDialect dialect)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                builder.Where(static x => x.Id <= 1);
                return builder.Build();
            }
        }
    }


    [TestMethod]
    public void Null()
    {
        var actual = createQuery(this.Dialect);
        var expect =
@"where
    [姓] is null";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().BeNull();

        static Query createQuery(DbDialect dialect)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                builder.Where(static x => x.LastName == null);
                return builder.Build();
            }
        }
    }


    [TestMethod]
    public void NotNull()
    {
        var actual = createQuery(this.Dialect);
        var expect =
@"where
    [姓] is not null";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().BeNull();

        static Query createQuery(DbDialect dialect)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                builder.Where(static x => x.LastName != null);
                return builder.Build();
            }
        }
    }


    [TestMethod]
    public void And()
    {
        var actual = createQuery(this.Dialect);
        var expect =
@"where
    [Id] > @p1 and [姓] = @p2";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain("p1", 1);
        actual.Parameters.Should().Contain("p2", "xin9le");

        static Query createQuery(DbDialect dialect)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                builder.Where(static x => x.Id > 1 && x.LastName == "xin9le");
                return builder.Build();
            }
        }
    }


    [TestMethod]
    public void Or()
    {
        var actual = createQuery(this.Dialect);
        var expect =
@"where
    [Id] > @p1 or [姓] = @p2";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain("p1", 1);
        actual.Parameters.Should().Contain("p2", "xin9le");

        static Query createQuery(DbDialect dialect)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                builder.Where(static x => x.Id > 1 || x.LastName == "xin9le");
                return builder.Build();
            }
        }
    }


    [TestMethod]
    public void AndOr1()
    {
        var actual = createQuery(this.Dialect);
        var expect =
@"where
    ([Id] > @p1 and [姓] = @p2) or [Age] <= @p3";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain("p1", 1);
        actual.Parameters.Should().Contain("p2", "xin9le");
        actual.Parameters.Should().Contain("p3", 30);

        static Query createQuery(DbDialect dialect)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                builder.Where(static x => x.Id > 1 && x.LastName == "xin9le" || x.Age <= 30);
                return builder.Build();
            }
        }
    }


    [TestMethod]
    public void AndOr2()
    {
        var actual = createQuery(this.Dialect);
        var expect =
@"where
    [Id] > @p1 and ([姓] = @p2 or [Age] <= @p3)";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain("p1", 1);
        actual.Parameters.Should().Contain("p2", "xin9le");
        actual.Parameters.Should().Contain("p3", 30);

        static Query createQuery(DbDialect dialect)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                builder.Where(static x => x.Id > 1 && (x.LastName == "xin9le" || x.Age <= 30));
                return builder.Build();
            }
        }
    }


    [TestMethod]
    public void AndOr3()
    {
        var actual = createQuery(this.Dialect);
        var expect =
@"where
    [Id] > @p1 or ([姓] = @p2 and [Age] <= @p3)";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain("p1", 1);
        actual.Parameters.Should().Contain("p2", "xin9le");
        actual.Parameters.Should().Contain("p3", 30);

        static Query createQuery(DbDialect dialect)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                builder.Where(static x => x.Id > 1 || x.LastName == "xin9le" && x.Age <= 30);
                return builder.Build();
            }
        }
    }


    [TestMethod]
    public void AndOr4()
    {
        var actual = createQuery(this.Dialect);
        var expect =
@"where
    ([Id] > @p1 or [姓] = @p2) and [Age] <= @p3";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain("p1", 1);
        actual.Parameters.Should().Contain("p2", "xin9le");
        actual.Parameters.Should().Contain("p3", 30);

        static Query createQuery(DbDialect dialect)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                builder.Where(static x => (x.Id > 1 || x.LastName == "xin9le") && x.Age <= 30);
                return builder.Build();
            }
        }
    }


    [TestMethod]
    public void AndOr5()
    {
        var value1 = Enumerable.Range(0, 1000).ToArray();
        var value2 = Enumerable.Range(1000, 234).ToArray();
        var actual = createQuery(this.Dialect, value1, value2);
        var expect =
@"where
    ([Id] > @p1 or [姓] = @p2) and [Age] <= @p3 and ([Id] in @p4 or [Id] in @p5)";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain("p1", 1);
        actual.Parameters.Should().Contain("p2", "xin9le");
        actual.Parameters.Should().Contain("p3", 30);
        actual.Parameters.Should().ContainKey("p4");
        actual.Parameters!["p4"].Should().BeEquivalentTo(value1);
        actual.Parameters.Should().ContainKey("p5");
        actual.Parameters!["p5"].Should().BeEquivalentTo(value2);

        static Query createQuery(DbDialect dialect, int[] value1, int[] value2)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                var values = value1.Concat(value2);
                builder.Where(x => (x.Id > 1 || x.LastName == "xin9le") && x.Age <= 30 && values.Contains(x.Id));
                return builder.Build();
            }
        }
    }


    [TestMethod]
    public void AndOr6()
    {
        var value1 = Enumerable.Range(0, 1000).ToArray();
        var value2 = Enumerable.Range(1000, 234).ToArray();
        var actual = createQuery(this.Dialect, value1, value2);
        var expect =
@"where
    (([Id] > @p1 or [姓] = @p2) and [Age] <= @p3) or ([Id] in @p4 or [Id] in @p5)";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain("p1", 1);
        actual.Parameters.Should().Contain("p2", "xin9le");
        actual.Parameters.Should().Contain("p3", 30);
        actual.Parameters.Should().ContainKey("p4");
        actual.Parameters!["p4"].Should().BeEquivalentTo(value1);
        actual.Parameters.Should().ContainKey("p5");
        actual.Parameters!["p5"].Should().BeEquivalentTo(value2);

        static Query createQuery(DbDialect dialect, int[] value1, int[] value2)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                var values = value1.Concat(value2);
                builder.Where(x => (x.Id > 1 || x.LastName == "xin9le") && x.Age <= 30 || values.Contains(x.Id));
                return builder.Build();
            }
        }
    }


    [TestMethod]
    public void AndOr7()
    {
        var actual = createQuery(this.Dialect);
        var expect =
@"where
    (([Id] > @p1 or [姓] = @p2) and [Age] <= @p3) or 1 = 0";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain("p1", 1);
        actual.Parameters.Should().Contain("p2", "xin9le");
        actual.Parameters.Should().Contain("p3", 30);

        static Query createQuery(DbDialect dialect)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                var values = System.Array.Empty<int>();
                builder.Where(x => (x.Id > 1 || x.LastName == "xin9le") && x.Age <= 30 || values.Contains(x.Id));
                return builder.Build();
            }
        }
    }


    [TestMethod]
    public void AndOr8()
    {
        var actual = createQuery(this.Dialect);
        var expect =
@"where
    ([Id] > @p1 or [姓] = @p2) and ([Age] <= @p3 or 1 = 0)";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain("p1", 1);
        actual.Parameters.Should().Contain("p2", "xin9le");
        actual.Parameters.Should().Contain("p3", 30);

        static Query createQuery(DbDialect dialect)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                var values = System.Array.Empty<int>();
                builder.Where(x => (x.Id > 1 || x.LastName == "xin9le") && (x.Age <= 30 || values.Contains(x.Id)));
                return builder.Build();
            }
        }
    }


    [TestMethod]
    public void Contains_IEnumerable()
    {
        var values = Enumerable.Range(0, 3).ToArray();
        var actual = createQuery(this.Dialect, values);
        var expect =
@"where
    [Id] in @p1";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().ContainKey("p1");
        actual.Parameters!["p1"].Should().BeEquivalentTo(values);

        static Query createQuery(DbDialect dialect, int[] values)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                builder.Where(x => values.Contains(x.Id));
                return builder.Build();
            }
        }
    }


    [TestMethod]
    public void Contains_IEnumerable_Over1000()
    {
        var value1 = Enumerable.Range(0, 1000).ToArray();
        var value2 = Enumerable.Range(1000, 234).ToArray();
        var actual = createQuery(this.Dialect, value1, value2);
        var expect =
@"where
    ([Id] in @p1 or [Id] in @p2)";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().ContainKey("p1");
        actual.Parameters!["p1"].Should().BeEquivalentTo(value1);
        actual.Parameters.Should().ContainKey("p2");
        actual.Parameters!["p2"].Should().BeEquivalentTo(value2);

        static Query createQuery(DbDialect dialect, int[] value1, int[] value2)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                var values = value1.Concat(value2);
                builder.Where(x => values.Contains(x.Id));
                return builder.Build();
            }
        }
    }


    [TestMethod]
    public void Contains_IEnumerable_NoElements()
    {
        var actual = createQuery(this.Dialect);
        var expect =
@"where
    1 = 0";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().BeNull();

        static Query createQuery(DbDialect dialect)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                var values = System.Array.Empty<int>();
                builder.Where(x => values.Contains(x.Id));
                return builder.Build();
            }
        }
    }


    [TestMethod]
    public void Contains_ConcreteType()
    {
        var value1 = Enumerable.Range(0, 1000).ToArray();
        var value2 = Enumerable.Range(1000, 234).ToArray();
        var actual = createQuery(this.Dialect, value1, value2);
        var expect =
@"where
    ([Id] in @p1 or [Id] in @p2)";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().ContainKey("p1");
        actual.Parameters!["p1"].Should().BeEquivalentTo(value1);
        actual.Parameters.Should().ContainKey("p2");
        actual.Parameters!["p2"].Should().BeEquivalentTo(value2);

        static Query createQuery(DbDialect dialect, int[] value1, int[] value2)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                var values = value1.Concat(value2).ToHashSet();
                builder.Where(x => values.Contains(x.Id));
                return builder.Build();
            }
        }
    }


    [TestMethod]
    public void Variable()
    {
        var id = 1;
        var actual = createQuery(this.Dialect, id);
        var expect =
@"where
    [Id] = @p1";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain("p1", id);

        static Query createQuery(DbDialect dialect, int id)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                builder.Where(x => x.Id == id);
                return builder.Build();
            }
        }
    }


    [TestMethod]
    public void Constructor()
    {
        var actual = createQuery(this.Dialect);
        var expect =
@"where
    [姓] = @p1";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain("p1", "aaa");

        static Query createQuery(DbDialect dialect)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                builder.Where(static x => x.LastName == new string('a', 3));
                return builder.Build();
            }
        }
    }


    [TestMethod]
    public void Array()
    {
        // do nothing
    }


    [TestMethod]
    public void InstanceMethod()
    {
        var some = new AccessorProvider();
        var actual = createQuery(this.Dialect, some);
        var expect =
@"where
    [姓] = @p1";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain("p1", some.InstanceMethod());

        static Query createQuery(DbDialect dialect, AccessorProvider some)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                builder.Where(x => x.LastName == some.InstanceMethod());
                return builder.Build();
            }
        }
    }


    [TestMethod]
    public void Lambda()
    {
        var actual = createQuery(this.Dialect);
        var expect =
@"where
    [姓] = @p1";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain("p1", "123");

        static Query createQuery(DbDialect dialect)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                Func<int, string> getName = static x => x.ToString(CultureInfo.InvariantCulture);
                builder.Where(x => x.LastName == getName(123));
                return builder.Build();
            }
        }
    }


    [TestMethod]
    public void InstanceProperty()
    {
        var some = new AccessorProvider();
        var actual = createQuery(this.Dialect, some);
        var expect =
@"where
    [Age] = @p1";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain("p1", some.InstanceProperty);

        static Query createQuery(DbDialect dialect, AccessorProvider some)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                builder.Where(x => x.Age == some.InstanceProperty);
                return builder.Build();
            }
        }
    }


    [TestMethod]
    public void Indexer()
    {
        var ids = new[] { 1, 2, 3 };
        var actual = createQuery(this.Dialect, ids);
        var expect =
@"where
    [Id] = @p1";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain("p1", ids[0]);

        static Query createQuery(DbDialect dialect, int[] ids)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                builder.Where(x => x.Id == ids[0]);
                return builder.Build();
            }
        }
    }


    [TestMethod]
    public void StaticMethod()
    {
        var actual = createQuery(this.Dialect);
        var expect =
@"where
    [姓] = @p1";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain("p1", AccessorProvider.StaticMethod());

        static Query createQuery(DbDialect dialect)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                builder.Where(static x => x.LastName == AccessorProvider.StaticMethod());
                return builder.Build();
            }
        }
    }


    [TestMethod]
    public void StaticProperty()
    {
        var actual = createQuery(this.Dialect);
        var expect =
@"where
    [Age] = @p1";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain("p1", AccessorProvider.StaticProperty);

        static Query createQuery(DbDialect dialect)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                builder.Where(static x => x.Age == AccessorProvider.StaticProperty);
                return builder.Build();
            }
        }
    }


    [TestMethod]
    public void Enum_AsVariable()
    {
        var actual = createQuery(this.Dialect);
        var expect =
@"where
    [Sex] = @p1";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain("p1", Sex.Male);

        static Query createQuery(DbDialect dialect)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                var sex = Sex.Male;  // as variable
                builder.Where(x => x.Sex == sex);
                return builder.Build();
            }
        }
    }


    [TestMethod]
    public void Enum_AsConstant()
    {
        var actual = createQuery(this.Dialect);
        var expect =
@"where
    [Sex] = @p1";
        actual.Text.Should().Be(expect);
        actual.Parameters.Should().NotBeNull();
        actual.Parameters.Should().Contain("p1", (byte)Sex.Female);

        static Query createQuery(DbDialect dialect)
        {
            using (var builder = new QueryBuilder<Person>(dialect))
            {
                builder.Where(static x => x.Sex == Sex.Female);
                return builder.Build();
            }
        }
    }
}
