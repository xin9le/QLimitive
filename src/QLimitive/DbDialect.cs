using System.Collections.Generic;
using System.Linq;

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
    /// Gets the default schema.
    /// </summary>
    public string? DefaultSchema { get; }
    #endregion


    #region Constructors
    /// <summary>
    /// Creates instance.
    /// </summary>
    /// <param name="database"></param>
    /// <param name="bindParameterPrefix"></param>
    /// <param name="keywordBracket"></param>
    /// <param name="defaultSchema"></param>
    private DbDialect(DbKind database, char bindParameterPrefix, in BracketPair keywordBracket, string? defaultSchema)
    {
        this.Database = database;
        this.BindParameterPrefix = bindParameterPrefix;
        this.KeywordBracket = keywordBracket;
        this.DefaultSchema = defaultSchema;
    }


    /// <summary>
    /// Call when first access.
    /// </summary>
    static DbDialect()
    {
        All = new[]
        {
            SqlServer,
            MySql,
            Sqlite,
            PostgreSql,
            Oracle,
        };
        ByDatabase = All.ToDictionary(static x => x.Database);
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
    public static IReadOnlyDictionary<DbKind, DbDialect> ByDatabase { get; }


    /// <summary>
    /// Gets the SQL Server dialect.
    /// </summary>
    public static DbDialect SqlServer { get; } = new
    (
        DbKind.SqlServer,
        '@',
        new BracketPair('[', ']'),
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
        null
    );
    #endregion
}
