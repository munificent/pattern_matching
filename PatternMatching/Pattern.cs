using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// An F#-like pattern-matching system in C#, by Bob Nystrom.

namespace PatternMatching
{
    /// <summary>
    /// Static accessor class for pattern matching. Use this instead of
    /// constructing a Matcher{T} directly.
    /// </summary>
    public class Pattern
    {
        /// <summary>
        /// Begins a pattern matching block for a match that does not return a result.
        /// </summary>
        /// <typeparam name="T">Type of value being matched.</typeparam>
        /// <param name="value">The value being matched against.</param>
        /// <returns>The Matcher that does the matching.</returns>
        public static Matcher<T> Match<T>(T value)
        {
            return new Matcher<T>(value);
        }

        /// <summary>
        /// Begins a pattern matching block for a match that does return a result.
        /// </summary>
        /// <typeparam name="T">Type of value being matched.</typeparam>
        /// <param name="value">The value being matched against.</param>
        /// <returns>The Matcher that does the matching.</returns>
        public static ReturnMatcher<TValue, TResult> Match<TValue, TResult>(TValue value)
        {
            return new ReturnMatcher<TValue, TResult>(value);
        }
    }
}
