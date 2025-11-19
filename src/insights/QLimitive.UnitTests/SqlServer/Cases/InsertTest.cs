using Microsoft.VisualStudio.TestTools.UnitTesting;
using QLimitive.UnitTests.SqlServer.Models;
using Shouldly;

namespace QLimitive.UnitTests.SqlServer.Cases;



[TestClass]
public sealed class InsertTest
{
    private DbDialect Dialect { get; } = DbDialect.SqlServer;


    [TestMethod]
    public void ReferencePropertyValue()
    {
        var actual = QueryBuilder.Insert<Person>(this.Dialect, useAmbientValue: false);
        var expect =
@"insert into [dbo].[T_People]
(
    [姓],
    [名],
    [Age],
    [Sex],
    [HasChildren],
    [CreatedAt],
    [UpdatedAt]
)
values
(
    @LastName,
    @FirstName,
    @Age,
    @Sex,
    @HasChildren,
    @CreatedAt,
    @ModifiedAt
)";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("LastName", null);
        actual.Parameters.ShouldContainKeyAndValue("FirstName", null);
        actual.Parameters.ShouldContainKeyAndValue("Age", null);
        actual.Parameters.ShouldContainKeyAndValue("Sex", null);
        actual.Parameters.ShouldContainKeyAndValue("HasChildren", null);
        actual.Parameters.ShouldContainKeyAndValue("CreatedAt", null);
        actual.Parameters.ShouldContainKeyAndValue("ModifiedAt", null);
    }


    [TestMethod]
    public void ReferenceAmbientValue()
    {
        var actual = QueryBuilder.Insert<Person>(this.Dialect, useAmbientValue: true);
        var expect =
@"insert into [dbo].[T_People]
(
    [姓],
    [名],
    [Age],
    [Sex],
    [HasChildren],
    [CreatedAt],
    [UpdatedAt]
)
values
(
    @LastName,
    @FirstName,
    @Age,
    @Sex,
    @HasChildren,
    SYSDATETIME(),
    SYSDATETIME()
)";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("LastName", null);
        actual.Parameters.ShouldContainKeyAndValue("FirstName", null);
        actual.Parameters.ShouldContainKeyAndValue("Age", null);
        actual.Parameters.ShouldContainKeyAndValue("Sex", null);
        actual.Parameters.ShouldContainKeyAndValue("HasChildren", null);
    }
}
