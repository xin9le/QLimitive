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
    public BindParameterCollection? Parameters { get; }
    #endregion


    #region Constructors
    /// <summary>
    /// Creates instance.
    /// </summary>
    internal Query(string text, BindParameterCollection? parameters)
    {
        this.Text = text;
        this.Parameters = parameters;
    }


    /// <summary>
    /// Deconstruct into text and parameters.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="parameters"></param>
    public void Deconstruct(out string text, out BindParameterCollection? parameters)
    {
        text = this.Text;
        parameters = this.Parameters;
    }
    #endregion
}
