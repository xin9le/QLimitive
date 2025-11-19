using System;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QLimitive.UnitTests.SqlServer.Models;
using Shouldly;

namespace QLimitive.UnitTests.SqlServer.Cases;



[TestClass]
public sealed class WhereTest
{
    #region Fields
    private static readonly DbDialect s_dialect = DbDialect.SqlServer;
    #endregion


    [TestMethod]
    public void Equal()
    {
        var actual = createQuery();
        var expect =
@"where
    [Id] = @p1";

        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("p1", 1);

        #region Local Functions
        static Query createQuery()
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                builder.Where(static x => x.Id == 1);
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void NotEqual()
    {
        var actual = createQuery();
        var expect =
@"where
    [Id] <> @p1";

        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("p1", 1);

        #region Local Functions
        static Query createQuery()
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                builder.Where(static x => x.Id != 1);
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void GreaterThan()
    {
        var actual = createQuery();
        var expect =
@"where
    [Id] > @p1";

        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("p1", 1);

        #region Local Functions
        static Query createQuery()
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                builder.Where(static x => x.Id > 1);
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void LessThan()
    {
        var actual = createQuery();
        var expect =
@"where
    [Id] < @p1";

        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("p1", 1);

        #region Local Functions
        static Query createQuery()
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                builder.Where(static x => x.Id < 1);
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void GreaterThanOrEqual()
    {
        var actual = createQuery();
        var expect =
@"where
    [Id] >= @p1";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("p1", 1);

        #region Local Functions
        static Query createQuery()
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                builder.Where(static x => x.Id >= 1);
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void LessThanOrEqual()
    {
        var actual = createQuery();
        var expect =
@"where
    [Id] <= @p1";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("p1", 1);

        #region Local Functions
        static Query createQuery()
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                builder.Where(static x => x.Id <= 1);
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void Null()
    {
        var actual = createQuery();
        var expect =
@"where
    [姓] is null";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldBeNull();

        #region Local Functions
        static Query createQuery()
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                builder.Where(static x => x.LastName == null);
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void NotNull()
    {
        var actual = createQuery();
        var expect =
@"where
    [姓] is not null";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldBeNull();

        #region Local Functions
        static Query createQuery()
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                builder.Where(static x => x.LastName != null);
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void And()
    {
        var actual = createQuery();
        var expect =
@"where
    [Id] > @p1 and [姓] = @p2";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("p1", 1);
        actual.Parameters.ShouldContainKeyAndValue("p2", "xin9le");

        #region Local Functions
        static Query createQuery()
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                builder.Where(static x => x.Id > 1 && x.LastName == "xin9le");
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void Or()
    {
        var actual = createQuery();
        var expect =
@"where
    [Id] > @p1 or [姓] = @p2";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("p1", 1);
        actual.Parameters.ShouldContainKeyAndValue("p2", "xin9le");

        #region Local Functions
        static Query createQuery()
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                builder.Where(static x => x.Id > 1 || x.LastName == "xin9le");
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void AndOr1()
    {
        var actual = createQuery();
        var expect =
@"where
    ([Id] > @p1 and [姓] = @p2) or [Age] <= @p3";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("p1", 1);
        actual.Parameters.ShouldContainKeyAndValue("p2", "xin9le");
        actual.Parameters.ShouldContainKeyAndValue("p3", 30);

        #region Local Functions
        static Query createQuery()
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                builder.Where(static x => x.Id > 1 && x.LastName == "xin9le" || x.Age <= 30);
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void AndOr2()
    {
        var actual = createQuery();
        var expect =
@"where
    [Id] > @p1 and ([姓] = @p2 or [Age] <= @p3)";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("p1", 1);
        actual.Parameters.ShouldContainKeyAndValue("p2", "xin9le");
        actual.Parameters.ShouldContainKeyAndValue("p3", 30);

        #region Local Functions
        static Query createQuery()
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                builder.Where(static x => x.Id > 1 && (x.LastName == "xin9le" || x.Age <= 30));
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void AndOr3()
    {
        var actual = createQuery();
        var expect =
@"where
    [Id] > @p1 or ([姓] = @p2 and [Age] <= @p3)";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("p1", 1);
        actual.Parameters.ShouldContainKeyAndValue("p2", "xin9le");
        actual.Parameters.ShouldContainKeyAndValue("p3", 30);

        #region Local Functions
        static Query createQuery()
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                builder.Where(static x => x.Id > 1 || x.LastName == "xin9le" && x.Age <= 30);
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void AndOr4()
    {
        var actual = createQuery();
        var expect =
@"where
    ([Id] > @p1 or [姓] = @p2) and [Age] <= @p3";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("p1", 1);
        actual.Parameters.ShouldContainKeyAndValue("p2", "xin9le");
        actual.Parameters.ShouldContainKeyAndValue("p3", 30);

        #region Local Functions
        static Query createQuery()
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                builder.Where(static x => (x.Id > 1 || x.LastName == "xin9le") && x.Age <= 30);
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void AndOr5()
    {
        var value1 = Enumerable.Range(0, 1000).ToArray();
        var value2 = Enumerable.Range(1000, 234).ToArray();
        var actual = createQuery(value1, value2);
        var expect =
@"where
    ([Id] > @p1 or [姓] = @p2) and [Age] <= @p3 and ([Id] in @p4 or [Id] in @p5)";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("p1", 1);
        actual.Parameters.ShouldContainKeyAndValue("p2", "xin9le");
        actual.Parameters.ShouldContainKeyAndValue("p3", 30);
        actual.Parameters.ShouldContainKey("p4");
        actual.Parameters!["p4"].ShouldBe(value1);
        actual.Parameters.ShouldContainKey("p5");
        actual.Parameters!["p5"].ShouldBe(value2);

        #region Local Functions
        static Query createQuery(int[] value1, int[] value2)
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                var values = value1.Concat(value2);
                builder.Where(x => (x.Id > 1 || x.LastName == "xin9le") && x.Age <= 30 && values.Contains(x.Id));
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void AndOr6()
    {
        var value1 = Enumerable.Range(0, 1000).ToArray();
        var value2 = Enumerable.Range(1000, 234).ToArray();
        var actual = createQuery(value1, value2);
        var expect =
@"where
    (([Id] > @p1 or [姓] = @p2) and [Age] <= @p3) or ([Id] in @p4 or [Id] in @p5)";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("p1", 1);
        actual.Parameters.ShouldContainKeyAndValue("p2", "xin9le");
        actual.Parameters.ShouldContainKeyAndValue("p3", 30);
        actual.Parameters.ShouldContainKey("p4");
        actual.Parameters!["p4"].ShouldBe(value1);
        actual.Parameters.ShouldContainKey("p5");
        actual.Parameters!["p5"].ShouldBe(value2);

        #region Local Functions
        static Query createQuery(int[] value1, int[] value2)
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                var values = value1.Concat(value2);
                builder.Where(x => (x.Id > 1 || x.LastName == "xin9le") && x.Age <= 30 || values.Contains(x.Id));
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void AndOr7()
    {
        var actual = createQuery();
        var expect =
@"where
    (([Id] > @p1 or [姓] = @p2) and [Age] <= @p3) or 1 = 0";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("p1", 1);
        actual.Parameters.ShouldContainKeyAndValue("p2", "xin9le");
        actual.Parameters.ShouldContainKeyAndValue("p3", 30);

        #region Local Functions
        static Query createQuery()
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                var values = System.Array.Empty<int>();
                builder.Where(x => (x.Id > 1 || x.LastName == "xin9le") && x.Age <= 30 || values.Contains(x.Id));
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void AndOr8()
    {
        var actual = createQuery();
        var expect =
@"where
    ([Id] > @p1 or [姓] = @p2) and ([Age] <= @p3 or 1 = 0)";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("p1", 1);
        actual.Parameters.ShouldContainKeyAndValue("p2", "xin9le");
        actual.Parameters.ShouldContainKeyAndValue("p3", 30);

        #region Local Functions
        static Query createQuery()
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                var values = System.Array.Empty<int>();
                builder.Where(x => (x.Id > 1 || x.LastName == "xin9le") && (x.Age <= 30 || values.Contains(x.Id)));
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void Contains_IEnumerable()
    {
        var values = Enumerable.Range(0, 3).ToArray();
        var actual = createQuery(values);
        var expect =
@"where
    [Id] in @p1";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKey("p1");
        actual.Parameters!["p1"].ShouldBe(values);

        #region Local Functions
        static Query createQuery(int[] values)
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                builder.Where(x => values.Contains(x.Id));
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void Contains_IEnumerable_Over1000()
    {
        var value1 = Enumerable.Range(0, 1000).ToArray();
        var value2 = Enumerable.Range(1000, 234).ToArray();
        var actual = createQuery(value1, value2);
        var expect =
@"where
    ([Id] in @p1 or [Id] in @p2)";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKey("p1");
        actual.Parameters!["p1"].ShouldBe(value1);
        actual.Parameters.ShouldContainKey("p2");
        actual.Parameters!["p2"].ShouldBe(value2);

        #region Local Functions
        static Query createQuery(int[] value1, int[] value2)
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                var values = value1.Concat(value2);
                builder.Where(x => values.Contains(x.Id));
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void Contains_IEnumerable_NoElements()
    {
        var actual = createQuery();
        var expect =
@"where
    1 = 0";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldBeNull();

        #region Local Functions
        static Query createQuery()
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                var values = System.Array.Empty<int>();
                builder.Where(x => values.Contains(x.Id));
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void Contains_ConcreteType()
    {
        var value1 = Enumerable.Range(0, 1000).ToArray();
        var value2 = Enumerable.Range(1000, 234).ToArray();
        var actual = createQuery(value1, value2);
        var expect =
@"where
    ([Id] in @p1 or [Id] in @p2)";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKey("p1");
        actual.Parameters!["p1"].ShouldBe(value1);
        actual.Parameters.ShouldContainKey("p2");
        actual.Parameters!["p2"].ShouldBe(value2);

        #region Local Functions
        static Query createQuery(int[] value1, int[] value2)
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                var values = value1.Concat(value2).ToHashSet();
                builder.Where(x => values.Contains(x.Id));
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void Variable()
    {
        var id = 1;
        var actual = createQuery(id);
        var expect =
@"where
    [Id] = @p1";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("p1", id);

        #region Local Functions
        static Query createQuery(int id)
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                builder.Where(x => x.Id == id);
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void Constructor()
    {
        var actual = createQuery();
        var expect =
@"where
    [姓] = @p1";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("p1", "aaa");

        #region Local Functions
        static Query createQuery()
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                builder.Where(static x => x.LastName == new string('a', 3));
                return builder.Build();
            }
        }
        #endregion
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
        var actual = createQuery(some);
        var expect =
@"where
    [姓] = @p1";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("p1", some.InstanceMethod());

        #region Local Functions
        static Query createQuery(AccessorProvider some)
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                builder.Where(x => x.LastName == some.InstanceMethod());
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void Lambda()
    {
        var actual = createQuery();
        var expect =
@"where
    [姓] = @p1";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("p1", "123");

        #region Local Functions
        static Query createQuery()
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                Func<int, string> getName = static x => x.ToString(CultureInfo.InvariantCulture);
                builder.Where(x => x.LastName == getName(123));
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void InstanceProperty()
    {
        var some = new AccessorProvider();
        var actual = createQuery(some);
        var expect =
@"where
    [Age] = @p1";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("p1", some.InstanceProperty);

        #region Local Functions
        static Query createQuery(AccessorProvider some)
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                builder.Where(x => x.Age == some.InstanceProperty);
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void Indexer()
    {
        var ids = new[] { 1, 2, 3 };
        var actual = createQuery(ids);
        var expect =
@"where
    [Id] = @p1";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("p1", ids[0]);

        #region Local Functions
        static Query createQuery(int[] ids)
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                builder.Where(x => x.Id == ids[0]);
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void StaticMethod()
    {
        var actual = createQuery();
        var expect =
@"where
    [姓] = @p1";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("p1", AccessorProvider.StaticMethod());

        #region Local Functions
        static Query createQuery()
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                builder.Where(static x => x.LastName == AccessorProvider.StaticMethod());
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void StaticProperty()
    {
        var actual = createQuery();
        var expect =
@"where
    [Age] = @p1";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("p1", AccessorProvider.StaticProperty);

        #region Local Functions
        static Query createQuery()
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                builder.Where(static x => x.Age == AccessorProvider.StaticProperty);
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void Enum_AsVariable()
    {
        var actual = createQuery();
        var expect =
@"where
    [Sex] = @p1";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("p1", Sex.Male);

        #region Local Functions
        static Query createQuery()
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                var sex = Sex.Male;  // as variable
                builder.Where(x => x.Sex == sex);
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void Enum_AsConstant()
    {
        var actual = createQuery();
        var expect =
@"where
    [Sex] = @p1";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("p1", (int)Sex.Female);

        #region Local Functions
        static Query createQuery()
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                builder.Where(static x => x.Sex == Sex.Female);
                return builder.Build();
            }
        }
        #endregion
    }
}
