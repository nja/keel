using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Keel.Objects
{
    public class Lambda : Function
    {
        private readonly Cons body;

        public Lambda(Cons lambdaList, Cons body)
            : base(lambdaList.Select(car => car.As<Symbol>()), body)
        {
            this.body = body;
        }

        public override LispObject Apply(IEnumerable<LispObject> argumentValues, LispEnvironment env)
        {
            LispEnvironment lambdaEnv = new LispEnvironment(env);
            lambdaEnv.Extend(Arguments, argumentValues);

            return lambdaEnv.Eval(body);
        }

        public override string ToString()
        {
            return string.Format("(LAMBDA ({0}) ...)", string.Join(" ", Arguments));
        }
    }
}
