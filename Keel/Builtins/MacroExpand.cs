using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Objects;

namespace Keel.Builtins
{
    public class Macro : LispObject
    {
        private readonly Lambda expander;
        private readonly LispEnvironment macroEnv;

        public Macro(LispObject lambdaList, Cons macroBody, LispEnvironment macroEnv)
        {
            this.expander = new Lambda(lambdaList, macroBody, macroEnv);
            this.macroEnv = macroEnv;
        }

        public Cons Expand(Cons form, LispEnvironment env)
        {
            var argumentValues = form.Cdr.As<Cons>();
            return expander.Apply(argumentValues, macroEnv).As<Cons>();
        }

        public override string ToString()
        {
            return "MACRO";
        }
    }

    public class MacroExpand : Builtin
    {
        public MacroExpand()
            : base("MACROEXPAND", "FORM")
        { }

        public override LispObject Apply(Cons argumentValues, LispEnvironment env)
        {
            Cons expansion;

            Expand(argumentValues.Car.As<Cons>(), env, out expansion);

            return expansion;
        }

        public static bool Expand(Cons form, LispEnvironment env, out Cons expansion)
        {
            bool expanded = false;
            Cons toExpand = form;

            while (ExpandOnce(toExpand, env, out expansion)) 
            {
                toExpand = expansion;
                expanded = true;
            }

            return expanded;
        }

        public static bool ExpandOnce(Cons form, LispEnvironment env, out Cons expansion)
        {
            if (env.IsMacro(form))
            {
                var macro = env.LookUp((Symbol)form.Car).As<Macro>();

                expansion = macro.Expand(form, env);

                return true;
            }
            else
            {
                expansion = form;
                return false;
            }
        }
    }
}
