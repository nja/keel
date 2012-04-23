namespace Keel
{
    using System.Collections.Generic;

    using Keel.Builtins;
    using Keel.Objects;
    using Keel.SpecialForms;

    public class Specials
    {
        #region Constants and Fields

        private readonly Dictionary<string, SpecialForm> specials = new Dictionary<string, SpecialForm>();

        #endregion

        #region Constructors and Destructors

        public Specials()
        {
            Add(new Define());
            Add(new Defmacro());
            Add(new Defun());
            Add(new Do());
            Add(new DoStar());
            Add(new If());
            Add(new LambdaForm());
            Add(new Progn());
            Add(new Quote());
            Add(new SetForm());
        }

        #endregion

        #region Public Methods and Operators

        public LispObject Eval(Cons form, LispEnvironment env)
        {
            if (!IsSpecial(form))
            {
                throw new SpecialFormException(form + " is not a special form");
            }

            var specialSymbol = (Symbol)Car.Of(form);
            var special = specials[specialSymbol.Name];
            var body = (Cons)Cdr.Of(form);

            return special.Eval(body, env);
        }

        public bool IsSpecial(Cons form)
        {
            var car = Car.Of(form);
            var cdr = Cdr.Of(form);

            return car is Symbol
                   && cdr is Cons
                   && specials.ContainsKey(car.As<Symbol>().Name);
        }

        #endregion

        #region Methods

        private void Add(SpecialForm special)
        {
            specials.Add(special.Name, special);
        }

        #endregion
    }
}
