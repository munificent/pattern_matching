using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// An F#-like pattern-matching system in C#, by Bob Nystrom.

namespace PatternMatching
{
    /// <summary>
    /// Fluent interface class for handling pattern matching cases that do not return a value.
    /// </summary>
    /// <typeparam name="T">Type of value to match.</typeparam>
    public class Matcher<T>
    {
        #region Match on type

        /// <summary>
        /// Match that performs the given Action if the value is the given type.
        /// </summary>
        /// <typeparam name="TCase">The type of value to match.</typeparam>
        /// <param name="action">The action to perform if the value matches. May be null
        /// in order to match and do nothing but prevent further matches.</param>
        /// <returns>This Matcher or a NullMatcher if the match succeeded.</returns>
        public Matcher<T> Case<TCase>(Action action)
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
        public Matcher<T> Case<TCase>(Action<T> action)
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
        public Matcher<T> Case<TCase, TArg>(Action<TArg> action)
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
        public Matcher<T> Case<TCase, TArg1, TArg2>(Action<TArg1, TArg2> action)
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
        public Matcher<T> Case<TCase, TArg1, TArg2, TArg3>(
            Action<TArg1, TArg2, TArg3> action)
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
        public Matcher<T> Case<TCase, TArg1, TArg2, TArg3, TArg4>(
            Action<TArg1, TArg2, TArg3, TArg4> action)
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
        /// Match that performs the given Action if the value is the equivalent to the
        /// given test value.
        /// </summary>
        /// <param name="value">The value to compare the match value with.</param>
        /// <param name="action">The action to perform if the value matches. May be null
        /// in order to match and do nothing but prevent further matches.</param>
        /// <returns>This Matcher or a NullMatcher if the match succeeded.</returns>
        public Matcher<T> Case(T value, Action action)
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
        public Matcher<T> Case(T value, Action<T> action)
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
        /// <param name="action">The action to perform if the predicate passes. May be null
        /// in order to match and do nothing but prevent further matches.</param>
        /// <returns>This Matcher or a NullMatcher if the match succeeded.</returns>
        public virtual Matcher<T> Case(Func<bool> predicate, Action action)
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
        public virtual Matcher<T> Case(Func<T, bool> predicate, Action action)
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
        public virtual Matcher<T> Case(Func<bool> predicate, Action<T> action)
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
        public virtual Matcher<T> Case(Func<T, bool> predicate, Action<T> action)
        {
            if (predicate(mValue))
            {
                // allow null matches
                if (action != null) action(mValue);

                return new NullMatcher<T>();
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
        /// <param name="action">The action to perform if the predicate passes. May be null
        /// in order to match and do nothing but prevent further matches.</param>
        /// <returns>This Matcher or a NullMatcher if the match succeeded.</returns>
        public Matcher<T> Default(Action action)
        {
            return Case(() => true, action);
        }

        /// <summary>
        /// Default match that always succeeds.
        /// </summary>
        /// <param name="action">The action to perform if the predicate passes. May be null
        /// in order to match and do nothing but prevent further matches.</param>
        /// <returns>This Matcher or a NullMatcher if the match succeeded.</returns>
        public Matcher<T> Default(Action<T> action)
        {
            return Case(() => true, action);
        }

        /// <summary>
        /// Initializes a new instance of Matcher that selects a match using the given value.
        /// </summary>
        /// <param name="value">The value to match.</param>
        /// <remarks>Marked internal so that users use Pattern.Match() instead of constructing this
        /// directly.</remarks>
        internal Matcher(T value)
        {
            mValue = value;
        }

        /// <summary>
        /// This is only used by NullMatcher.
        /// </summary>
        internal Matcher() { }

        private T mValue;
    }
}
