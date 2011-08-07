using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keel.Objects;
using Keel.Builtins;

namespace Keel.SpecialForms
{
    public class SpecialFormException : Exception
    {
        public SpecialFormException(string msg)
            : base(msg)
        { }
    }

    public abstract class SpecialForm
    {
        private readonly string name;
        
        protected SpecialForm(string name)
        {
            this.name = name;
        }

        public string Name { get { return name; } }

        public abstract LispObject Eval(Cons body, LispEnvironment env);
    }
}
