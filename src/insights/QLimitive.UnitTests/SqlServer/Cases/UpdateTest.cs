using Microsoft.VisualStudio.TestTools.UnitTesting;
using QLimitive.UnitTests.SqlServer.Models;
using Shouldly;

namespace QLimitive.UnitTests.SqlServer.Cases;



[TestClass]
public sealed class UpdateTest
{
    #region Fields
    private static readonly DbDialect s_dialect = DbDialect.SqlServer;
    #endregion


    [TestMethod]
    public void AllColumns()
    {
        var actual = QueryBuilder.Update<Person>(s_dialect);
        var expect =
@"update [dbo].[T_People]
set
    [Id] = @Id,
    [姓] = @LastName,
    [名] = @FirstName,
    [Age] = @Age,
    [Sex] = @Sex,
    [HasChildren] = @HasChildren,
    [CreatedAt] = @CreatedAt,
    [UpdatedAt] = @ModifiedAt";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("Id", null);
        actual.Parameters.ShouldContainKeyAndValue("LastName", null);
        actual.Parameters.ShouldContainKeyAndValue("FirstName", null);
        actual.Parameters.ShouldContainKeyAndValue("Age", null);
        actual.Parameters.ShouldContainKeyAndValue("Sex", null);
        actual.Parameters.ShouldContainKeyAndValue("HasChildren", null);
        actual.Parameters.ShouldContainKeyAndValue("CreatedAt", null);
        actual.Parameters.ShouldContainKeyAndValue("ModifiedAt", null);
    }


    [TestMethod]
    public void AllColumns_UseAmbientValue()
    {
        var actual = QueryBuilder.Update<Person>(s_dialect, useAmbientValue: true);
        var expect =
@"update [dbo].[T_People]
set
    [Id] = @Id,
    [姓] = @LastName,
    [名] = @FirstName,
    [Age] = @Age,
    [Sex] = @Sex,
    [HasChildren] = @HasChildren,
    [CreatedAt] = SYSDATETIME(),
    [UpdatedAt] = SYSDATETIME()";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("Id", null);
        actual.Parameters.ShouldContainKeyAndValue("LastName", null);
        actual.Parameters.ShouldContainKeyAndValue("FirstName", null);
        actual.Parameters.ShouldContainKeyAndValue("Age", null);
        actual.Parameters.ShouldContainKeyAndValue("Sex", null);
        actual.Parameters.ShouldContainKeyAndValue("HasChildren", null);
    }


    [TestMethod]
    public void OneColumn()
    {
        var actual = QueryBuilder.Update<Person>(s_dialect, static x => x.LastName);
        var expect =
@"update [dbo].[T_People]
set
    [姓] = @LastName";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("LastName", null);
    }


    [TestMethod]
    public void OneColumn_AnonymousType()
    {
        var actual = QueryBuilder.Update<Person>(s_dialect, static x => new { x.LastName });
        var expect =
@"update [dbo].[T_People]
set
    [姓] = @LastName";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("LastName", null);
    }


    [TestMethod]
    public void MultiColumns()
    {
        var actual = QueryBuilder.Update<Person>(s_dialect, static x => new { x.LastName, x.FullName, x.ModifiedAt });
        var expect =
@"update [dbo].[T_People]
set
    [姓] = @LastName,
    [UpdatedAt] = @ModifiedAt";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("LastName", null);
        actual.Parameters.ShouldContainKeyAndValue("ModifiedAt", null);
    }


    [TestMethod]
    public void MultiColumns_UseAmbientValue()
    {
        var actual = QueryBuilder.Update<Person>(s_dialect, static x => new { x.LastName, x.FullName, x.ModifiedAt }, useAmbientValue: true);
        var expect =
@"update [dbo].[T_People]
set
    [姓] = @LastName,
    [UpdatedAt] = SYSDATETIME()";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("LastName", null);
    }
}
