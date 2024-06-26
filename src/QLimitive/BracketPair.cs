﻿namespace QLimitive;



/// <summary>
/// Represents a begin/end bracket pair.
/// </summary>
public readonly struct BracketPair
{
    #region Properties
    /// <summary>
    /// Gets the begin bracket.
    /// </summary>
    public char Begin { get; }


    /// <summary>
    /// Gets the end bracket.
    /// </summary>
    public char End { get; }
    #endregion


    #region Constructors
    /// <summary>
    /// Creates instance.
    /// </summary>
    /// <param name="begin"></param>
    /// <param name="end"></param>
    internal BracketPair(char begin, char end)
    {
        this.Begin = begin;
        this.End = end;
    }


    /// <summary>
    /// Deconstruct into begin and end.
    /// </summary>
    /// <param name="begin"></param>
    /// <param name="end"></param>
    public void Deconstruct(out char begin, out char end)
    {
        begin = this.Begin;
        end = this.End;
    }
    #endregion
}
