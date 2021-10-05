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
    public class NullReturnMatcher<TValue, TResult> : ReturnMatcher<TValue, TResult>
    {
        /// <summary>
        /// Gets the result of the match.
        /// </summary>
        public override TResult Result { get { return mResult; } }

        public override ReturnMatcher<TValue, TResult> Case(Func<bool> predicate, Func<TResult> action) { return this; }
        public override ReturnMatcher<TValue, TResult> Case(Func<TValue, bool> predicate, Func<TResult> action) { return this; }
        public override ReturnMatcher<TValue, TResult> Case(Func<bool> predicate, Func<TValue, TResult> action) { return this; }
        public override ReturnMatcher<TValue, TResult> Case(Func<TValue, bool> predicate, Func<TValue, TResult> action) { return this; }

        /// <summary>
        /// Marked internal because only ReturnMatcher should construct.
        /// </summary>
        internal NullReturnMatcher(TResult result)
        {
            mResult = result;
        }

        private TResult mResult;
    }
}
