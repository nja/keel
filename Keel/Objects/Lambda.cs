namespace Keel.Objects
{
    public class Lambda : Function
    {
        private readonly string name;
        private readonly Cons body;
        private readonly LispEnvironment closure;

        public Lambda(LispObject lambdaList, Cons body, LispEnvironment closure)
            : this(null, lambdaList, body, closure)
        { }

        public Lambda(string name, LispObject lambdaList, Cons body, LispEnvironment closure)
            : base(lambdaList)
        {
            this.name = name;
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
            if (name == null)
            {
                return string.Format("<Anynomous function: {0}>", ArgumentsString);
            }
            
            return string.Format("<Named function {0}: {1}>", this.name, this.ArgumentsString);
        }
    }
}
