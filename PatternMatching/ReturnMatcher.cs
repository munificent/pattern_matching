using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// An F#-like pattern-matching system in C#, by Bob Nystrom.

namespace PatternMatching
{
    /// <summary>
    /// Fluent interface class for handling pattern matching cases that do return a value.
    /// </summary>
    /// <typeparam name="TValue">Type of value to match.</typeparam>
    /// <typeparam name="TResult">Type of value to return.</typeparam>
    public class ReturnMatcher<TValue, TResult>
    {
        /// <summary>
        /// Implicit conversion to the result value. Allows you to assign the result
        /// of a pattern matching block directly to a variable of the result type
        /// without explicitly calling .Result at the end of the chain.
        /// </summary>
        /// <param name="matcher">The match.</param>
        /// <returns>The result of the match.</returns>
        public static implicit operator TResult(ReturnMatcher<TValue, TResult> matcher)
        {
            return matcher.Result;
        }

        /// <summary>
        /// Gets the result of the match. Since Result on this type will only be called
        /// if no matches have succeeded always NoMatchException.
        /// </summary>
        public virtual TResult Result { get { throw new NoMatchException(String.Format("Could not find a match for {0}.", mValue)); } }

        #region Match on type

        /// <summary>
        /// Match that returns the given result if the value is the given type.
        /// </summary>
        /// <typeparam name="TCase">The type of value to match.</typeparam>
        /// <param name="action">The action to perform if the value matches.</param>
        /// <param name="result">The result to return if the match is successful.</param>
        /// <returns>This Matcher or a NullMatcher if the match succeeded.</returns>
        public ReturnMatcher<TValue, TResult> Case<TCase>(TResult result)
        {
            return Case(() => mValue is TCase, result);
        }

        /// <summary>
        /// Match that performs the given Action if the value is the given type.
        /// </summary>
        /// <typeparam name="TCase">The type of value to match.</typeparam>
        /// <param name="action">The action to perform if the value matches.</param>
        /// <returns>This Matcher or a NullMatcher if the match succeeded.</returns>
        public ReturnMatcher<TValue, TResult> Case<TCase>(Func<TResult> action)
        {
            return Case(() => mValue is TCase, action);
        }

        /// <summary>
        /// Match that performs the given Action if the value is the given type.
        /// </summary>
        /// <typeparam name="TCase">The type of value to match.</typeparam>
        /// <param name="action">The action to perform if the value matches. May be null
        /// in order to match and do nothing but prevent further matches.</param>
        /// <returns>This Matcher or a NullMatcher if the match succeeded.</returns>
        public ReturnMatcher<TValue, TResult> Case<TCase>(Func<TValue, TResult> action)
        {
            return Case(() => mValue is TCase, action);
        }

        #endregion

        #region Match on type with extracted fields

        /// <summary>
        /// Match that performs the given Action if the value is the given type and
        /// exposes a field.
        /// </summary>
        /// <typeparam name="TCase">The type of value to match.</typeparam>
        /// <param name="action">The action to perform if the value matches. May be null
        /// in order to match and do nothing but prevent further matches.</param>
        /// <returns>This Matcher or a NullMatcher if the match succeeded.</returns>
        public ReturnMatcher<TValue, TResult> Case<TCase, TArg>(Func<TArg, TResult> action)
        {
            IMatchable<TArg> matchable = mValue as IMatchable<TArg>;

            return Case(() => (matchable != null) && (mValue is TCase),
                        () => action(matchable.GetArg()));
        }

        /// <summary>
        /// Match that performs the given Action if the value is the given type and
        /// exposes two fields.
        /// </summary>
        /// <typeparam name="TCase">The type of value to match.</typeparam>
        /// <param name="action">The action to perform if the value matches. May be null
        /// in order to match and do nothing but prevent further matches.</param>
        /// <returns>This Matcher or a NullMatcher if the match succeeded.</returns>
        public ReturnMatcher<TValue, TResult> Case<TCase, TArg1, TArg2>(Func<TArg1, TArg2, TResult> action)
        {
            IMatchable<TArg1, TArg2> matchable = mValue as IMatchable<TArg1, TArg2>;

            return Case(() => (matchable != null) && (mValue is TCase),
                        () => action(matchable.GetArg1(), matchable.GetArg2()));
        }

        /// <summary>
        /// Match that performs the given Action if the value is the given type and
        /// exposes three fields.
        /// </summary>
        /// <typeparam name="TCase">The type of value to match.</typeparam>
        /// <param name="action">The action to perform if the value matches. May be null
        /// in order to match and do nothing but prevent further matches.</param>
        /// <returns>This Matcher or a NullMatcher if the match succeeded.</returns>
        public ReturnMatcher<TValue, TResult> Case<TCase, TArg1, TArg2, TArg3>(
            Func<TArg1, TArg2, TArg3, TResult> action)
        {
            IMatchable<TArg1, TArg2, TArg3> matchable =
                mValue as IMatchable<TArg1, TArg2, TArg3>;

            return Case(() => (matchable != null) && (mValue is TCase),
                        () => action(matchable.GetArg1(),
                                     matchable.GetArg2(),
                                     matchable.GetArg3()));
        }

        /// <summary>
        /// Match that performs the given Action if the value is the given type and
        /// exposes four fields.
        /// </summary>
        /// <typeparam name="TCase">The type of value to match.</typeparam>
        /// <param name="action">The action to perform if the value matches. May be null
        /// in order to match and do nothing but prevent further matches.</param>
        /// <returns>This Matcher or a NullMatcher if the match succeeded.</returns>
        public ReturnMatcher<TValue, TResult> Case<TCase, TArg1, TArg2, TArg3, TArg4>(
            Func<TArg1, TArg2, TArg3, TArg4, TResult> action)
        {
            IMatchable<TArg1, TArg2, TArg3, TArg4> matchable =
                mValue as IMatchable<TArg1, TArg2, TArg3, TArg4>;

            return Case(() => (matchable != null) && (mValue is TCase),
                        () => action(matchable.GetArg1(),
                                     matchable.GetArg2(),
                                     matchable.GetArg3(),
                                     matchable.GetArg4()));
        }

        #endregion

        #region Match on value

        /// <summary>
        /// Match that returns the given result if the value is the equivalent to the
        /// given test value.
        /// </summary>
        /// <param name="value">The value to compare the match value with.</param>
        /// <param name="result">The result to return if the match is successful.</param>
        /// <returns>This Matcher or a NullMatcher if the match succeeded.</returns>
        public virtual ReturnMatcher<TValue, TResult> Case(TValue value, TResult result)
        {
            return Case(() => Equals(mValue, value), () => result);
        }

        /// <summary>
        /// Match that performs the given Action if the value is the equivalent to the
        /// given test value.
        /// </summary>
        /// <param name="value">The value to compare the match value with.</param>
        /// <param name="action">The action to perform if the value matches. May be null
        /// in order to match and do nothing but prevent further matches.</param>
        /// <returns>This Matcher or a NullMatcher if the match succeeded.</returns>
        public virtual ReturnMatcher<TValue, TResult> Case(TValue value, Func<TResult> action)
        {
            return Case(() => Equals(mValue, value), action);
        }

        /// <summary>
        /// Match that performs the given Action if the value is the equivalent to the
        /// given test value.
        /// </summary>
        /// <param name="value">The value to compare the match value with.</param>
        /// <param name="action">The action to perform if the value matches. May be null
        /// in order to match and do nothing but prevent further matches.</param>
        /// <returns>This Matcher or a NullMatcher if the match succeeded.</returns>
        public virtual ReturnMatcher<TValue, TResult> Case(TValue value, Func<TValue, TResult> action)
        {
            return Case(() => Equals(mValue, value),
                        () => action(mValue));
        }

        #endregion

        #region Match on a predicate

        /// <summary>
        /// Match that performs the given Action if the given predicate returns true.
        /// </summary>
        /// <param name="predicate">The predicate to evaluate to test the match.</param>
        /// <param name="result">The result to return if the match is successful.</param>
        /// <returns>This Matcher or a NullMatcher if the match succeeded.</returns>
        public virtual ReturnMatcher<TValue, TResult> Case(Func<bool> predicate, TResult result)
        {
            return Case(ignore => predicate(), ignore => result);
        }

        /// <summary>
        /// Match that performs the given Action if the given predicate returns true.
        /// </summary>
        /// <param name="predicate">The predicate to evaluate to test the match.</param>
        /// <param name="result">The result to return if the match is successful.</param>
        /// <returns>This Matcher or a NullMatcher if the match succeeded.</returns>
        public virtual ReturnMatcher<TValue, TResult> Case(Func<TValue, bool> predicate, TResult result)
        {
            return Case(predicate, ignore => result);
        }

        /// <summary>
        /// Match that performs the given Action if the given predicate returns true.
        /// </summary>
        /// <param name="predicate">The predicate to evaluate to test the match.</param>
        /// <param name="action">The action to perform if the predicate passes. May be null
        /// in order to match and do nothing but prevent further matches.</param>
        /// <returns>This Matcher or a NullMatcher if the match succeeded.</returns>
        public virtual ReturnMatcher<TValue, TResult> Case(Func<bool> predicate, Func<TResult> action)
        {
            return Case(ignore => predicate(), ignore => action());
        }

        /// <summary>
        /// Match that performs the given Action if the given predicate returns true.
        /// </summary>
        /// <param name="predicate">The predicate to evaluate to test the match.</param>
        /// <param name="action">The action to perform if the predicate passes. May be null
        /// in order to match and do nothing but prevent further matches.</param>
        /// <returns>This Matcher or a NullMatcher if the match succeeded.</returns>
        public virtual ReturnMatcher<TValue, TResult> Case(Func<TValue, bool> predicate, Func<TResult> action)
        {
            return Case(predicate, ignore => action());
        }

        /// <summary>
        /// Match that performs the given Action if the given predicate returns true.
        /// </summary>
        /// <param name="predicate">The predicate to evaluate to test the match.</param>
        /// <param name="action">The action to perform if the predicate passes. May be null
        /// in order to match and do nothing but prevent further matches.</param>
        /// <returns>This Matcher or a NullMatcher if the match succeeded.</returns>
        public virtual ReturnMatcher<TValue, TResult> Case(Func<bool> predicate, Func<TValue, TResult> action)
        {
            return Case(ignore => predicate(), action);
        }

        /// <summary>
        /// Match that performs the given Action if the given predicate returns true.
        /// </summary>
        /// <param name="predicate">The predicate to evaluate to test the match.</param>
        /// <param name="action">The action to perform if the predicate passes. May be null
        /// in order to match and do nothing but prevent further matches.</param>
        /// <returns>This Matcher or a NullMatcher if the match succeeded.</returns>
        public virtual ReturnMatcher<TValue, TResult> Case(Func<TValue, bool> predicate, Func<TValue, TResult> action)
        {
            if (predicate(mValue))
            {
                return new NullReturnMatcher<TValue, TResult>(action(mValue));
            }
            else
            {
                return this;
            }
        }

        #endregion

        /// <summary>
        /// Default match that always succeeds.
        /// </summary>
        /// <param name="result">The result to return.</param>
        /// <returns>This Matcher or a NullMatcher if the match succeeded.</returns>
        public ReturnMatcher<TValue, TResult> Default(TResult result)
        {
            return Case(() => true, () => result);
        }

        /// <summary>
        /// Default match that always succeeds.
        /// </summary>
        /// <param name="action">The action to perform if the predicate passes. May be null
        /// in order to match and do nothing but prevent further matches.</param>
        /// <returns>This Matcher or a NullMatcher if the match succeeded.</returns>
        public ReturnMatcher<TValue, TResult> Default(Func<TResult> action)
        {
            return Case(() => true, action);
        }

        /// <summary>
        /// Default match that always succeeds.
        /// </summary>
        /// <param name="action">The action to perform if the predicate passes. May be null
        /// in order to match and do nothing but prevent further matches.</param>
        /// <returns>This Matcher or a NullMatcher if the match succeeded.</returns>
        public ReturnMatcher<TValue, TResult> Default(Func<TValue, TResult> action)
        {
            return Case(() => true, action);
        }

        /// <summary>
        /// Initializes a new instance of Matcher that selects a match using the given value.
        /// </summary>
        /// <param name="value">The value to match.</param>
        /// <remarks>Marked internal so that users use Pattern.Match() instead of constructing this
        /// directly.</remarks>
        internal ReturnMatcher(TValue value)
        {
            mValue = value;
        }

        /// <summary>
        /// This is only used by NullMatcher.
        /// </summary>
        internal ReturnMatcher() { }

        private TValue mValue;
    }
}
