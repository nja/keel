namespace Keel.Builtins
{
    using System.Collections.Generic;
    using System.Data;

    using Keel.Objects;

    public class Divide : Builtin
    {
        public Divide()
            : base("/")
        { }

        public override LispObject Apply(Cons arguments, LispEnvironment env)
        {
            var argList = new List<LispObject>(arguments);

            if (argList.Count < 2)
            {
                throw new EvaluateException("Too few arguments to " + this.Name);
            }

            if (argList.Count > 2)
            {
                throw new EvaluateException("Too many arguments to " + this.Name);
            }

            return argList[0].As<LispNumber>().DivideBy(argList[1].As<LispNumber>());
        }
    }
}