using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// An F#-like pattern-matching system in C#, by Bob Nystrom.

namespace PatternMatching
{
    #region Image macro example classes

    abstract class ImageMacro { }

    class Lolcat : ImageMacro, IMatchable<string>
    {
        public string Caption;
        public Lolcat(string caption) { Caption = caption; }

        string IMatchable<string>.GetArg() { return Caption; }
    }

    class Lolrus : ImageMacro, IMatchable<int>
    {
        public int NumBuckets;
        public Lolrus(int numBuckets) { NumBuckets = numBuckets; }

        int IMatchable<int>.GetArg() { return NumBuckets; }
    }

    class Loldog : ImageMacro, IMatchable<string, int>
    {
        public string Caption;
        public int NumHotdogs;

        public Loldog(string caption, int numHotdogs)
        {
            Caption = caption;
            NumHotdogs = numHotdogs;
        }

        string IMatchable<string, int>.GetArg1() { return Caption; }
        int IMatchable<string, int>.GetArg2() { return NumHotdogs; }
    }

    class ORlyOwl : ImageMacro { }

    #endregion

    #region Expression example classes

    public class Expression
    {
        public static implicit operator Expression(int value) { return new Number(value); }
    }

    public class Number : Expression, IMatchable<int>
    {
        public int Value;
        public Number(int value) { Value = value; }

        int IMatchable<int>.GetArg() { return Value; }
    }

    public abstract class Operator : Expression, IMatchable<Expression, Expression>
    {
        public Expression Left;
        public Expression Right;

        public Operator(Expression left, Expression right)
        {
            Left = left;
            Right = right;
        }

        Expression IMatchable<Expression, Expression>.GetArg1() { return Left; }
        Expression IMatchable<Expression, Expression>.GetArg2() { return Right; }
    }

    public class Add : Operator
    {
        public Add(Expression left, Expression right) : base(left, right) {}
    }

    public class Multiply : Operator
    {
        public Multiply(Expression left, Expression right) : base(left, right) { }
    }

    #endregion

    public class Examples
    {
        public static void Run()
        {
            // match on type and extract fields
            ImageMacro image = new Lolcat("I made you a cookie");
            Pattern.Match(image).
                Case<Lolcat, string>        (caption            => Console.WriteLine("Lolcat says '" + caption + "'")).
                Case<Lolrus, int>           (buckets            => Console.WriteLine("Lolrus has " + buckets + " buckets")).
                Case<Loldog, string, int>   ((caption, hotdogs) => Console.WriteLine("Loldog says '" + caption + "' and has " + hotdogs + " hotdogs")).
                Case<ORlyOwl>               (()                 => Console.WriteLine("O RLY?"));

            // default case
            Pattern.Match(image).
                Case<Lolrus, int> (buckets => Console.WriteLine("Lolrus has " + buckets + " buckets")).
                Default           (()      => Console.WriteLine("Default"));

            // match on equality
            Pattern.Match("a string").
                Case("not",      () => Console.WriteLine("should not match")).
                Case("a string", () => Console.WriteLine("should match"));

            // match using a predicate
            Pattern.Match(123).
                Case(value => value < 100, () => Console.WriteLine("less than 100")).
                Case(value => value > 100, () => Console.WriteLine("greater than 100"));

            // simple match with a result
            bool isTwo = Pattern.Match<int, bool>(2).
                Case(2,  true).
                Default(false);

            Console.WriteLine("is two = " + isTwo);

            // nested with a result
            Expression expr = new Multiply(new Add(1, 2), new Add(3, 4));
            int result = Evaluate(expr);
            Console.WriteLine("expression evaluates to " + result);
        }

        private static int Evaluate(Expression expression)
        {
            return Pattern.Match<Expression, int>(expression).
                Case<Add, Expression, Expression>       ((left, right) => Evaluate(left) + Evaluate(right)).
                Case<Multiply, Expression, Expression>  ((left, right) => Evaluate(left) * Evaluate(right)).
                Case<Number, int>                       (value => value);
        }
    }
}
