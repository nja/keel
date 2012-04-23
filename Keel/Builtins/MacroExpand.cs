namespace Keel.Builtins
{
    using Keel.Objects;

    public class MacroExpand : Builtin
    {
        public MacroExpand()
            : base("MACROEXPAND", "FORM")
        { }

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
            
            expansion = form;
            return false;
        }

        public override LispObject Apply(Cons argumentValues, LispEnvironment env)
        {
            Cons expansion;

            Expand(argumentValues.Car.As<Cons>(), env, out expansion);

            return expansion;
        }
    }
}
