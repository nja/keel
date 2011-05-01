using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Keel.Objects
{
    public class Lambda : Function
    {
        private readonly Cons body;

        public Lambda(Cons args, Cons body)
            : base(args.Select(car => Assert<Symbol>(car)), body)
        {
            this.body = body;
        }

        public override LispObject Apply(IEnumerable<LispObject> argumentValues, LispEnvironment env)
        {
            LispEnvironment lambdaEnv = new LispEnvironment(env);
            lambdaEnv.Extend(Arguments, argumentValues);

            return env.Eval(body);
        }
    }
}
