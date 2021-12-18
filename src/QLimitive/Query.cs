namespace QLimitive;



/// <summary>
/// Represents a query text and bind parameter pair.
/// </summary>
public readonly struct Query
{
    #region Properties
    /// <summary>
    /// Gets the SQL text.
    /// </summary>
    public string Text { get; }


    /// <summary>
    /// Gets the bind parameter collection.
    /// </summary>
    public BindParameter? Parameters { get; }
    #endregion


    #region Constructors
    /// <summary>
    /// Creates instance.
    /// </summary>
    internal Query(string text, BindParameter? parameters)
    {
        this.Text = text;
        this.Parameters = parameters;
    }
    #endregion
}
