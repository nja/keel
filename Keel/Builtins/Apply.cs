using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Objects;

namespace Keel.Builtins
{
    public class ApplyBuiltin : Builtin
    {
        public static readonly ApplyBuiltin Instance = new ApplyBuiltin();

        private ApplyBuiltin()
            : base("APPLY")
        { }

        public override LispObject Apply(Cons argumentValues, LispEnvironment env)
        {
            var fun = argumentValues.Car.As<Function>();
            var args = argumentValues.Cdr.As<Cons>();
            
            var spread = Spread(args);

            return fun.Apply(spread, env);
        }

        public static Cons Spread(Cons args)
        {
            if (args.Cdr.IsNil)
            {
                return args.Car.As<Cons>();
            }
            else
            {
                return new Cons(args.Car, Spread(args.Cdr.As<Cons>()));
            }
        }
    }
}
