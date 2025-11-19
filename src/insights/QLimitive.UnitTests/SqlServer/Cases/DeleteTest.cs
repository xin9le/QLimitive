using Microsoft.VisualStudio.TestTools.UnitTesting;
using QLimitive.UnitTests.SqlServer.Models;
using Shouldly;

namespace QLimitive.UnitTests.SqlServer.Cases;



[TestClass]
public sealed class DeleteTest
{
    #region Fields
    private static readonly DbDialect s_dialect = DbDialect.SqlServer;
    #endregion


    [TestMethod]
    public void All()
    {
        var actual = QueryBuilder.Delete<Person>(s_dialect);
        var expect = "delete from [dbo].[T_People]";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldBeNull();
    }
}
