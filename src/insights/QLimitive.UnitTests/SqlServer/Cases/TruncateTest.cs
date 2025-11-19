using Microsoft.VisualStudio.TestTools.UnitTesting;
using QLimitive.UnitTests.SqlServer.Models;
using Shouldly;

namespace QLimitive.UnitTests.SqlServer.Cases;



[TestClass]
public sealed class TruncateTest
{
    #region Fields
    private static readonly DbDialect s_dialect = DbDialect.SqlServer;
    #endregion


    [TestMethod]
    public void Create()
    {
        var actual = QueryBuilder.Truncate<Person>(s_dialect);
        var expect = "truncate table [dbo].[T_People]";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldBeNull();
    }
}
