using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// An F#-like pattern-matching system in C#, by Bob Nystrom.

namespace PatternMatching
{
    /// <summary>
    /// Matcher that always fails a match.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NullMatcher<T> : Matcher<T>
    {
        public override Matcher<T> Case(Func<bool> predicate, Action action) { return this; }
        public override Matcher<T> Case(Func<T, bool> predicate, Action action) { return this; }
        public override Matcher<T> Case(Func<bool> predicate, Action<T> action) { return this; }
        public override Matcher<T> Case(Func<T, bool> predicate, Action<T> action) { return this; }
    }
}
