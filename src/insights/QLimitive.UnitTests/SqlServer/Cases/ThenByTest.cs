using Microsoft.VisualStudio.TestTools.UnitTesting;
using QLimitive.UnitTests.SqlServer.Models;
using Shouldly;

namespace QLimitive.UnitTests.SqlServer.Cases;



[TestClass]
public sealed class ThenByTest
{
    #region Fields
    private static readonly DbDialect s_dialect = DbDialect.SqlServer;
    #endregion


    [TestMethod]
    public void Ascending()
    {
        var actual = createQuery();
        var expect =
@"order by
    [姓],
    [Age]";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldBeNull();

        #region Local Functions
        static Query createQuery()
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                builder.OrderBy(static x => x.LastName);
                builder.ThenBy(static x => x.Age);
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void Descending()
    {
        var actual = createQuery();
        var expect =
@"order by
    [Age] desc,
    [名] desc";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldBeNull();

        #region Local Functions
        static Query createQuery()
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                builder.OrderByDescending(static x => x.Age);
                builder.ThenByDescending(static x => x.FirstName);
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void Complex()
    {
        var actual = createQuery();
        var expect =
@"order by
    [姓],
    [Age] desc,
    [名],
    [CreatedAt] desc";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldBeNull();

        #region Local Functions
        static Query createQuery()
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                builder.OrderBy(static x => x.LastName);
                builder.ThenByDescending(static x => x.Age);
                builder.ThenBy(static x => x.FirstName);
                builder.ThenByDescending(static x => x.CreatedAt);
                return builder.Build();
            }
        }
        #endregion
    }
}
