using Microsoft.VisualStudio.TestTools.UnitTesting;
using QLimitive.UnitTests.SqlServer.Models;
using Shouldly;

namespace QLimitive.UnitTests.SqlServer.Cases;



[TestClass]
public sealed class SelectTest
{
    #region Fields
    private static readonly DbDialect s_dialect = DbDialect.SqlServer;
    #endregion


    [TestMethod]
    public void AllColumns()
    {
        var actual = QueryBuilder.Select<Person>(s_dialect);
        var expect =
@"select
    [Id] as [Id],
    [姓] as [LastName],
    [名] as [FirstName],
    [Age] as [Age],
    [Sex] as [Sex],
    [HasChildren] as [HasChildren],
    [CreatedAt] as [CreatedAt],
    [UpdatedAt] as [ModifiedAt]
from [dbo].[T_People]";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldBeNull();
    }


    [TestMethod]
    public void OneColumn()
    {
        var actual = QueryBuilder.Select<Person>(s_dialect, static x => x.LastName);
        var expect =
@"select
    [姓] as [LastName]
from [dbo].[T_People]";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldBeNull();
    }


    [TestMethod]
    public void OneColumn_AnonymousType()
    {
        var actual = QueryBuilder.Select<Person>(s_dialect, static x => new { x.LastName });
        var expect =
@"select
    [姓] as [LastName]
from [dbo].[T_People]";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldBeNull();
    }


    [TestMethod]
    public void TwoColumns()
    {
        var actual = QueryBuilder.Select<Person>(s_dialect, static x => new { x.LastName, x.Age });
        var expect =
@"select
    [姓] as [LastName],
    [Age] as [Age]
from [dbo].[T_People]";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldBeNull();
    }


    [TestMethod]
    public void MultiColumns_IncludeNotMapped()
    {
        var actual = QueryBuilder.Select<Person>(s_dialect, static x => new { x.LastName, x.FullName, x.Age });
        var expect =
@"select
    [姓] as [LastName],
    [Age] as [Age]
from [dbo].[T_People]";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldBeNull();
    }
}
