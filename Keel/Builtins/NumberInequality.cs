namespace Keel.Builtins
{
    using System;

    using Keel.Objects;

    public class NumberInequality : Builtin
    {
        private readonly Func<LispNumber, LispNumber, bool> test;

        public NumberInequality(string name, Func<LispNumber, LispNumber, bool> test)
            : base(name)
        {
            this.test = test;
        }

        public override LispObject Apply(Cons arguments, LispEnvironment env)
        {
            if (arguments.IsNil)
            {
                throw new EvaluationException("Too few arguments to " + Name);
            }

            var previous = arguments.Car.As<LispNumber>();

            foreach (var arg in arguments.Cdr.As<Cons>())
            {
                var num = arg.As<LispNumber>();

                if (!test(previous, num))
                {
                    return LispNull.Nil;
                }

                previous = num;
            }

            return T.True;
        }
    }
}
