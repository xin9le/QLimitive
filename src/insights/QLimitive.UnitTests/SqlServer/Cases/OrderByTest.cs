using Microsoft.VisualStudio.TestTools.UnitTesting;
using QLimitive.UnitTests.SqlServer.Models;
using Shouldly;

namespace QLimitive.UnitTests.SqlServer.Cases;



[TestClass]
public sealed class OrderByTest
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
    [姓]";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldBeNull();

        #region Local Functions
        static Query createQuery()
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                builder.OrderBy(static x => x.LastName);
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
    [Age] desc";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldBeNull();

        #region Local Functions
        static Query createQuery()
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                builder.OrderByDescending(static x => x.Age);
                return builder.Build();
            }
        }
        #endregion
    }
}
