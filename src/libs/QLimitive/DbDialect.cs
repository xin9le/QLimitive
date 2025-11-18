using System.Collections.Frozen;
using System.Collections.Generic;

namespace QLimitive;



/// <summary>
/// Provides database dialect.
/// </summary>
public readonly struct DbDialect
{
    #region Properties
    /// <summary>
    /// Gets the database kind.
    /// </summary>
    public DbKind Database { get; }


    /// <summary>
    /// Gets the bind parameter prefix.
    /// </summary>
    public char BindParameterPrefix { get; }


    /// <summary>
    /// Gets the SQL keyword bracket.
    /// </summary>
    public BracketPair KeywordBracket { get; }


    /// <summary>
    /// Gets the maximum number of elements that can be included in an in operator.
    /// </summary>
    public int InOperatorMaxCount { get; }


    /// <summary>
    /// Gets the default schema.
    /// </summary>
    public string? DefaultSchema { get; }
    #endregion


    #region Constructors
    /// <summary>
    /// Creates instance.
    /// </summary>
    private DbDialect(DbKind database, char bindParameterPrefix, in BracketPair keywordBracket, int inOperatorMaxCount, string? defaultSchema)
    {
        this.Database = database;
        this.BindParameterPrefix = bindParameterPrefix;
        this.KeywordBracket = keywordBracket;
        this.InOperatorMaxCount = inOperatorMaxCount;
        this.DefaultSchema = defaultSchema;
    }


    /// <summary>
    /// Call when first access.
    /// </summary>
    static DbDialect()
    {
        All = [
            SqlServer,
            MySql,
            Sqlite,
            PostgreSql,
            Oracle,
        ];
        ByDatabase = All.ToFrozenDictionary(static x => x.Database);
    }
    #endregion


    #region Instances
    /// <summary>
    /// Gets all database dialects.
    /// </summary>
    public static IReadOnlyList<DbDialect> All { get; }


    /// <summary>
    /// Gets all database dialects by <see cref="DbKind"/>.
    /// </summary>
    public static FrozenDictionary<DbKind, DbDialect> ByDatabase { get; }


    /// <summary>
    /// Gets the SQL Server dialect.
    /// </summary>
    public static DbDialect SqlServer { get; } = new
    (
        DbKind.SqlServer,
        '@',
        new BracketPair('[', ']'),
        1000,
        "dbo"
    );


    /// <summary>
    /// Gets the MySQL / Amazon Aurora / MariaDB dialect.
    /// </summary>
    public static DbDialect MySql { get; } = new
    (
        DbKind.MySql,
        '@',
        new BracketPair('`', '`'),
        1000,
       null
    );


    /// <summary>
    /// Gets the SQLite dialect.
    /// </summary>
    public static DbDialect Sqlite { get; } = new
    (
        DbKind.Sqlite,
        '@',
        new BracketPair('"', '"'),
        1000,
        null
    );


    /// <summary>
    /// Gets the PostgreSQL dialect.
    /// </summary>
    public static DbDialect PostgreSql { get; } = new
    (
        DbKind.PostgreSql,
        ':',
        new BracketPair('"', '"'),
        1000,
        null
   );


    /// <summary>
    /// Gets the Oracle dialect.
    /// </summary>
    public static DbDialect Oracle { get; } = new
    (
        DbKind.Oracle,
        ':',
        new BracketPair('"', '"'),
        1000,
        null
    );
    #endregion
}
