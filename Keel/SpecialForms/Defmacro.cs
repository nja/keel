using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Objects;
using Keel.Builtins;

namespace Keel.SpecialForms
{
    public class Defmacro : SpecialForm
    {
        public static readonly Defmacro Instance = new Defmacro();

        private Defmacro()
            : base("DEFMACRO")
        { }

        public override LispObject Eval(Cons defmacroBody, LispEnvironment env)
        {
            var symbol = defmacroBody.Car.As<Symbol>();
            var lambdaList = Car.Of(Cdr.Of(defmacroBody));
            var macroBody = Cdr.Of(Cdr.Of(defmacroBody)).As<Cons>();
            var progn = Progn.Wrap(macroBody);

            var macro = new Macro(lambdaList, progn, env);

            env.AddBinding(symbol, macro);

            return macro;
        }
    }
}
