namespace Keel.SpecialForms
{
    using System.Collections.Generic;
    using System.Linq;

    using Keel.Objects;

    public class Do : SpecialForm
    {
        public Do()
            : base("DO")
        { }

        protected Do(string name)
            : base(name)
        { }

        public override LispObject Eval(Cons body, LispEnvironment env)
        {
            var varForms = body.Car.As<Cons>();
            var varSymbols = varForms.Select(x => x.As<Cons>().Car.As<Symbol>()).ToList();
            var varInitForms = varForms.Select(x => x.As<Cons>().Cdr.As<Cons>().Car).ToList();
            var varStepForms = varForms.Select(x => x.As<Cons>().Cdr.As<Cons>().Cdr.As<Cons>().Car).ToList();

            var cadr = body.Cdr.As<Cons>().Car.As<Cons>();
            var testForm = cadr.Car;
            var resultForms = cadr.Cdr.As<Cons>();
            var doBody = body.Cdr.As<Cons>().Cdr.As<Cons>();

            var doEnv = new LispEnvironment(env);

            SetInitValues(doEnv, varSymbols, varInitForms);

            while (doEnv.Eval(testForm).IsNil)
            {
                new Progn().Eval(doBody, doEnv);

                SetStepValues(doEnv, varSymbols, varStepForms);
            }            

            return new Progn().Eval(resultForms, doEnv);
        }

        protected virtual void SetStepValues(LispEnvironment doEnv, List<Symbol> varSymbols, List<LispObject> varStepForms)
        {
            var stepValues = varStepForms.Select(doEnv.Eval).ToList();

            for (int i = 0; i < varSymbols.Count; i++)
            {
                doEnv.SetValue(varSymbols[i], stepValues[i]);
            }
        }

        protected virtual void SetInitValues(LispEnvironment doEnv, List<Symbol> varSymbols, List<LispObject> varInitForms)
        {
            var varInitValues = varInitForms.Select(doEnv.Eval).ToList();

            for (int i = 0; i < varSymbols.Count; i++)
            {
                doEnv.AddBinding(varSymbols[i], varInitValues[i]);
            }
        }
    }
}
