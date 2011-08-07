using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Keel.Objects
{
    public class Lambda : Function
    {
        private readonly Cons body;
        private readonly LispEnvironment closure;

        public Lambda(LispObject lambdaList, Cons body, LispEnvironment closure)
            : base(lambdaList, body)
        {
            this.body = body;
            this.closure = closure;
        }

        public override LispObject Apply(Cons argumentValues, LispEnvironment env)
        {
            LispEnvironment lambdaEnv = new LispEnvironment(closure);
            lambdaEnv.Extend(Arguments, argumentValues);

            return lambdaEnv.Eval(body);
        }

        public override string ToString()
        {
            return string.Format("(LAMBDA {0} ...)", Arguments.IsNil ? "()" : Arguments.ToString());
        }
    }
}
