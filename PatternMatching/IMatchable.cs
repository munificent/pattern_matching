using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// An F#-like pattern-matching system in C#, by Bob Nystrom.

namespace PatternMatching
{
    /// <summary>
    /// Exposes a single field from a type used in a pattern matching case.
    /// </summary>
    /// <typeparam name="TArg">Type of field being exposed.</typeparam>
    public interface IMatchable<TArg>
    {
        TArg GetArg();
    }

    /// <summary>
    /// Exposes two fields from a type used in a pattern matching case.
    /// </summary>
    /// <typeparam name="TArg1">Type of field being exposed.</typeparam>
    /// <typeparam name="TArg2">Type of field being exposed.</typeparam>
    public interface IMatchable<TArg1, TArg2>
    {
        TArg1 GetArg1();
        TArg2 GetArg2();
    }

    /// <summary>
    /// Exposes three fields from a type used in a pattern matching case.
    /// </summary>
    /// <typeparam name="TArg1">Type of field being exposed.</typeparam>
    /// <typeparam name="TArg2">Type of field being exposed.</typeparam>
    /// <typeparam name="TArg3">Type of field being exposed.</typeparam>
    public interface IMatchable<TArg1, TArg2, TArg3>
    {
        TArg1 GetArg1();
        TArg2 GetArg2();
        TArg3 GetArg3();
    }

    /// <summary>
    /// Exposes four fields from a type used in a pattern matching case.
    /// </summary>
    /// <typeparam name="TArg1">Type of field being exposed.</typeparam>
    /// <typeparam name="TArg2">Type of field being exposed.</typeparam>
    /// <typeparam name="TArg3">Type of field being exposed.</typeparam>
    /// <typeparam name="TArg4">Type of field being exposed.</typeparam>
    public interface IMatchable<TArg1, TArg2, TArg3, TArg4>
    {
        TArg1 GetArg1();
        TArg2 GetArg2();
        TArg3 GetArg3();
        TArg4 GetArg4();
    }
}
