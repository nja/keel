namespace Keel.SpecialForms
{
    using System.Collections.Generic;

    using Keel.Objects;

    public class DoStar : Do
    {
        public DoStar()
            : base("DO*")
        { }

        protected override void SetInitValues(LispEnvironment doEnv, List<Symbol> varSymbols, List<LispObject> varInitForms)
        {
            for (int i = 0; i < varSymbols.Count; i++)
            {
                var val = doEnv.Eval(varInitForms[i]);
                doEnv.AddBinding(varSymbols[i], val);
            }
        }

        protected override void SetStepValues(LispEnvironment doEnv, List<Symbol> varSymbols, List<LispObject> varStepForms)
        {
            for (int i = 0; i < varSymbols.Count; i++)
            {
                var val = doEnv.Eval(varStepForms[i]);
                doEnv.SetValue(varSymbols[i], val);
            }
        }
    }
}