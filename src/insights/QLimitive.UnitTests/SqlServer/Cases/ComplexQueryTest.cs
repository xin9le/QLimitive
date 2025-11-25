using Microsoft.VisualStudio.TestTools.UnitTesting;
using QLimitive.Internals;
using QLimitive.Mappings;
using QLimitive.UnitTests.SqlServer.Models;
using Shouldly;

namespace QLimitive.UnitTests.SqlServer.Cases;



[TestClass]
public sealed class ComplexQueryTest
{
    #region Fields
    private static readonly DbDialect s_dialect = DbDialect.SqlServer;
    #endregion


    [TestMethod]
    public void CountWhere1()
    {
        var actual = createQuery();
        var expect =
@"select count(*) as [Count] from [dbo].[T_People]
where
    [Id] = @p1";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("p1", 1);

        #region Local Functions
        static Query createQuery()
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                builder.Count();
                builder.Where(static x => x.Id == 1);
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void CountWhere2()
    {
        var actual = createQuery();
        var expect =
@"select count(*) as [Count] from [dbo].[T_People]
where
    [Id] = @p1 and [姓] <> @p2";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("p1", 1);
        actual.Parameters.ShouldContainKeyAndValue("p2", "xin9le");

        #region Local Functions
        static Query createQuery()
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                builder.Count();
                builder.Where(static x => x.Id == 1 && x.LastName != "xin9le");
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void CountWhere3()
    {
        var actual = createQuery();
        var expect =
@"select count(*) as [Count] from [dbo].[T_People]
where
    [Id] = @p1 or [姓] <> @p2";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("p1", 1);
        actual.Parameters.ShouldContainKeyAndValue("p2", "xin9le");

        #region Local Functions
        static Query createQuery()
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                builder.Count();
                builder.Where(static x => x.Id == 1 || x.LastName != "xin9le");
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void SelectWhere()
    {
        var actual = createQuery();
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
from [dbo].[T_People]
where
    [Id] = @p1 or [姓] <> @p2";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("p1", 1);
        actual.Parameters.ShouldContainKeyAndValue("p2", "xin9le");

        #region Local Functions
        static Query createQuery()
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                builder.Select();
                builder.Where(static x => x.Id == 1 || x.LastName != "xin9le");
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void SelectWhereOrderByThenBy()
    {
        var actual = createQuery();
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
from [dbo].[T_People]
where
    [Id] = @p1 or ([姓] <> @p2 and [Age] > @p3)
order by
    [Id],
    [Age] desc";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("p1", 1);
        actual.Parameters.ShouldContainKeyAndValue("p2", "xin9le");
        actual.Parameters.ShouldContainKeyAndValue("p3", 20);

        #region Local Functions
        static Query createQuery()
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                builder.Select();
                builder.Where(static x => x.Id == 1 || x.LastName != "xin9le" && x.Age > 20);
                builder.OrderBy(static x => x.Id);
                builder.ThenByDescending(static x => x.Age);
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void SelectWhereAsIsOrderByThenBy()
    {
        var actual = createQuery();
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
from [dbo].[T_People]
where
    [Id] = @p1 or ([姓] <> @p2 and [Age] > @p3)
    and [姓] like @term
order by
    [Id],
    [Age] desc";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("p1", 1);
        actual.Parameters.ShouldContainKeyAndValue("p2", "xin9le");
        actual.Parameters.ShouldContainKeyAndValue("p3", 20);
        actual.Parameters.ShouldContainKeyAndValue("term", "csharp");

        #region Local Functions
        static Query createQuery()
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                builder.Select();
                builder.Where(static x => x.Id == 1 || x.LastName != "xin9le" && x.Age > 20);
                builder.AsIs(static (ref stringBuilder, ref bindParameters, dialect) =>
                {
                    var term = "csharp";
                    var table = TableMappingInfo.Get<Person>();
                    var bracket = dialect.KeywordBracket;
                    var column = table.ColumnByMemberName[nameof(Person.LastName)];

                    stringBuilder.AppendLine();
                    stringBuilder.Append("    and ");
                    stringBuilder.Append(bracket.Begin);
                    stringBuilder.Append(column.ColumnName);
                    stringBuilder.Append(bracket.End);
                    stringBuilder.Append(" like ");
                    stringBuilder.Append(dialect.BindParameterPrefix);
                    stringBuilder.Append(nameof(term));

                    bindParameters ??= [];
                    bindParameters.Add(nameof(term), term);
                }, s_dialect);
                builder.OrderBy(static x => x.Id);
                builder.ThenByDescending(static x => x.Age);
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void UpdateWhere()
    {
        var actual = createQuery();
        var expect =
@"update [dbo].[T_People]
set
    [Age] = @Age,
    [UpdatedAt] = SYSDATETIME()
where
    [Id] = @p2 or [姓] <> @p3";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("Age", null);
        actual.Parameters.ShouldContainKeyAndValue("p2", 1);
        actual.Parameters.ShouldContainKeyAndValue("p3", "xin9le");

        #region Local Functions
        static Query createQuery()
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                builder.Update(static x => new { x.Age, x.ModifiedAt }, useAmbientValue: true);
                builder.Where(static x => x.Id == 1 || x.LastName != "xin9le");
                return builder.Build();
            }
        }
        #endregion
    }


    [TestMethod]
    public void DeleteWhere()
    {
        var actual = createQuery();
        var expect =
@"delete from [dbo].[T_People]
where
    [Id] = @p1 or [姓] <> @p2";
        actual.Text.ShouldBe(expect);
        actual.Parameters.ShouldNotBeNull();
        actual.Parameters.ShouldContainKeyAndValue("p1", 1);
        actual.Parameters.ShouldContainKeyAndValue("p2", "xin9le");

        #region Local Functions
        static Query createQuery()
        {
            using (var builder = new QueryBuilder<Person>(s_dialect))
            {
                builder.Delete();
                builder.Where(static x => x.Id == 1 || x.LastName != "xin9le");
                return builder.Build();
            }
        }
        #endregion
    }
}
